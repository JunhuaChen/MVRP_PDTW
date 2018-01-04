// Copyright ?Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Text;
using namespace Microsoft::SolverFoundation::Services;

/*
Production Capacity	
-------------------	
Country	     MaxProduction	Price
---------------------------------
Venezuela	   6000	          15
Saudi Arabia   9000	          20


Production Yield
----------------		
Country	       Product	     Yield
-----------------------------------
Venezuela	     gasoline	  0.4
Venezuela	     jet fuel	  0.2
Venezuela	     lubricant	  0.3
Saudi Arabia	 gasoline	  0.3
Saudi Arabia	 jet fuel	  0.4
Saudi Arabia	 lubricant    0.2


Production Requirements	
-----------------------	
Product	  MinRequirement	
------------------------
gasoline	2000	
jet fuel	1500	
lubricant	500	
*/

ref class PetroChem  {
private:

  /// <summary>
  /// A stub class for row in ProductionRequirements table
  /// </summary>
  ref class ProductionRequirements{
  public:
    property String^ Product;
    property double MinRequirements;

    ProductionRequirements(String^ product, double minRequired) {
      Product = product;
      MinRequirements = minRequired;
    }
  };

  /// <summary>
  /// A stub class for row in ProductionYield table
  /// </summary>
  ref class ProductionYield {
  public:
    property String^ Country;
    property String^ Product;
    property double Yield;

    ProductionYield(String^ country, String^ product, double yield) {
      Country = country;
      Product = product;
      Yield = yield;
    }
  };

  /// <summary>
  /// A stub class for row in ProductionCapacity table
  /// </summary>
  ref class ProductionCapacity {
  public:
    property String^ Country;
    property double Price;
    property double Production;
    property double MaxProduction;

    ProductionCapacity(String^ country, double maxProduction, double price) {
      Country = country;
      MaxProduction = maxProduction;
      Price = price;
      Production = -42;
    }
  };

  array< ProductionCapacity^ >^ _productionCapacity; 
  array< ProductionYield^ >^ _productionYield; 
  array< ProductionRequirements^ >^ _productionRequirements; 

  property IEnumerable<ProductionCapacity^>^  ProductionCapacityData {   
    IEnumerable<ProductionCapacity^>^ get(){
      return  _productionCapacity;
    };
  }
  property IEnumerable<ProductionYield^>^  ProductionYieldData {   
    IEnumerable<ProductionYield^>^ get(){
      return _productionYield;
    };
  }
  property IEnumerable<ProductionRequirements^>^  ProductionRequirementsData {   
    IEnumerable<ProductionRequirements^>^ get(){
      return _productionRequirements;
    };
  }

  Decision^ _buy;
  Parameter^ _max;
  Parameter^ _price;
  Parameter^ _yield;
  Parameter^ _min;
  Set^ _countries;
  Set^ _products;

  //This one is set by YieldConstraint delegate
  //so ProductionYieldTerm which needs it will know it
  Term ^ _currentProduct;

public:
  void Run() {

    _productionCapacity = gcnew array< ProductionCapacity^ >(2);
    _productionCapacity[0] = gcnew ProductionCapacity(L"Venezuela", 9000, 15);
    _productionCapacity[1] = gcnew ProductionCapacity(L"Saudi Arabia", 6000, 20);

    _productionYield = gcnew array< ProductionYield^ >(6);
    _productionYield[0] = gcnew ProductionYield(L"Venezuela", L"gasoline", 0.4);
    _productionYield[1] = gcnew ProductionYield(L"Venezuela", L"jet fuel", 0.2);
    _productionYield[2] = gcnew ProductionYield(L"Venezuela", L"lubricant", 0.3);
    _productionYield[3] = gcnew ProductionYield(L"Saudi Arabia", L"gasoline", 0.3);
    _productionYield[4] = gcnew ProductionYield(L"Saudi Arabia", L"jet fuel", 0.4);
    _productionYield[5] = gcnew ProductionYield(L"Saudi Arabia", L"lubricant", 0.2);

    _productionRequirements = gcnew array< ProductionRequirements^ >(3);
    _productionRequirements[0] = gcnew ProductionRequirements(L"gasoline", 2000);
    _productionRequirements[1] = gcnew ProductionRequirements(L"jet fuel", 1500);
    _productionRequirements[2] = gcnew ProductionRequirements(L"lubricant", 500);

    SolverContext^ context = SolverContext::GetContext();
    Model^ model = context->CreateModel();

    _countries = gcnew Set(Domain::Any, L"countries");
    _products = gcnew Set(Domain::Any, L"products");

    _buy = gcnew Decision(Domain::RealNonnegative, L"barrels", _countries);
    _buy->SetBinding(ProductionCapacityData, L"Production", L"Country");
    model->AddDecisions(_buy);

    _max = gcnew Parameter(Domain::RealNonnegative, L"max", _countries);
    _price = gcnew Parameter(Domain::RealNonnegative, L"price", _countries);
    _yield = gcnew Parameter(Domain::RealNonnegative, L"yield", _countries, _products);
    _min = gcnew Parameter(Domain::RealNonnegative, L"min", _products);
    _max->SetBinding(ProductionCapacityData, L"MaxProduction", L"Country");
    _price->SetBinding(ProductionCapacityData, L"Price", L"Country");
    _yield->SetBinding(ProductionYieldData, L"Yield", L"Country", L"Product");
    _min->SetBinding(ProductionRequirementsData, L"MinRequirements", L"Product");
    model->AddParameters(_max, _price, _yield, _min);

    model->AddConstraints(L"maxproduction",
      Model::ForEach(_countries, gcnew Func<Term^,Term^>(this, &PetroChem::ProductionLimit)));

    model->AddConstraints(L"yieldconstraint",
      Model::ForEach(_products, gcnew Func<Term^, Term^>(this, &PetroChem::YieldConstraint)));

    model->AddGoal(L"cost", GoalKind::Minimize,
      Model::Sum(Model::ForEach(_countries, gcnew Func<Term^,Term^>(this,  &PetroChem::Goal))));

    Solution^ solution = context->Solve(gcnew SimplexDirective());
    context->PropagateDecisions();
    Report^ report = solution->GetReport();
    Console::Write(L"{0}", report);

  }
private:

  Term^ ProductionLimit(Term^ country){
    return _buy[country] <= _max[country];
  }
  Term^ YieldConstraint(Term^ product){
    _currentProduct = product;
    return Model::Sum(Model::ForEach(_countries, 
      gcnew Func<Term^,Term^>(this, &PetroChem::ProductionYieldTerm))) >= _min[product];
  }
  Term^ ProductionYieldTerm(Term^ country){
    return _yield[country, _currentProduct] * _buy[country];
  }
  Term^ Goal(Term^ country){
    return _price[country] * _buy[country];
  }

};
int main(array<System::String ^> ^args)
{
  PetroChem^ petroChem = gcnew  PetroChem(); 
  petroChem->Run();
  return 0;
}
