// Copyright ?Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

///Problem Description
///--------------------------------------------------------------------------------------------------------------
/// A company produces bicycles for children. The sales forecast in thousand of units for the coming year are
///given in the following table. The company has a capacity of 30,000 bicycles per month. It is possible to
///augment the production by up to 50% through overtime working, but this increases the production cost
///for a bicycle from the usual $32 to $40.
///Table 8.1: Sales forecasts for the coming year in thousand units
///|************************************************|
///|Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec |
///|30  15  15  25  33  40  45  45  26  14  25  30  |
///|************************************************|
///Currently there are 2,000 bicycles in stock. The storage costs have been calculated as $5 per unit held in
///stock at the end of a month. We assume that the storage capacity at the company is virtually unlimited
///(in practice this means that the real capacity, that is quite obviously limited, does not impose any limits in
///our case). We are at the first of January. Which quantities need to be produced and stored in the course
///of the next twelve months in order to satisfy the forecast demand and minimize the total cost?
///---------------------------------------------------------------------------------------------------------------

#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <omp.h>
#include <algorithm>


int g_test_value[20];

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Text;
using namespace Microsoft::SolverFoundation::Services;

public ref class Demand {
public:
  property String^ Month ;
  property int Index ;
  property int Value ;

  Demand(String^ month, int index, int demand) {
    Month = month;
    Index = index;
    Value = demand;
  }
};
public ref class BicycleProduction {
private:
  //Initial stock
  int iCurrentStock;
  //Production cost per bicycle in normal hours
  double dCostNormal;
  //Production cost per bicycle in over time
  double dCostOt;
  //Storage cost per bicycle
  double dCostStock;
  //Maximum number of bicycles produced in normal hours
  int iNormalProdCap;
  //Maximum number of bicycles produced in over time
  int iOTProdCap;
  //Decisions and parameters
  Decision^ productionNormal;
  Decision^ productionOT;
  Decision^ store;
  Parameter^ pDemands;

  array< Demand^ >^ _monthlyForecast; 

  property IEnumerable<Demand^>^  DemandForecast {   
    IEnumerable<Demand^>^ get(){
      return  _monthlyForecast;
    };
  }

public:
  BicycleProduction() {
	 // inputData(12);
  }
  void inputData(int set_value)
  {
	  _monthlyForecast = gcnew array< Demand^ >(set_value);

	  for (int i = 0; i < set_value; i++)
	  {
		  _monthlyForecast[i] = gcnew Demand("ssss", i+1, g_test_value[i]);
	  }
	 /* _monthlyForecast[0] = gcnew Demand(L"Jan", 1, 30000);
	  _monthlyForecast[1] = gcnew Demand(L"Feb", 2, 15000);
	  _monthlyForecast[2] = gcnew Demand(L"Mar", 3, 15000);
	  _monthlyForecast[3] = gcnew Demand(L"Apr", 4, 25000);
	  _monthlyForecast[4] = gcnew Demand(L"May", 5, 33000);
	  _monthlyForecast[5] = gcnew Demand(L"Jun", 6, 40000);
	  _monthlyForecast[6] = gcnew Demand(L"Jul", 7, 45000);
	  _monthlyForecast[7] = gcnew Demand(L"Aug", 8, 45000);
	  _monthlyForecast[8] = gcnew Demand(L"Sep", 9, 26000);
	  _monthlyForecast[9] = gcnew Demand(L"Oct", 10, 14000);
	  _monthlyForecast[10] = gcnew Demand(L"Nov", 11, 25000);
	  _monthlyForecast[11] = gcnew Demand(L"Dec", 12, 30000);*/

	  iCurrentStock = 2000;
	  dCostNormal = 32;
	  dCostOt = 40;
	  dCostStock = 5;
	  iNormalProdCap = 30000;
	  iOTProdCap = 15000;
  }

  void Solve() {
    SolverContext^ context = SolverContext::GetContext();
    Model^ model = context->CreateModel();

    //Create an integer set for Months in the year
    Set^ setMonths = gcnew Set(Domain::IntegerRange(1, 12), L"Months");

    //Number of bicycles to be produces in normal hours for each month
    productionNormal = gcnew Decision(Domain::IntegerNonnegative, "ProductionNormal", setMonths);
    //Number of bicycles to be produces in over time for each month
    productionOT = gcnew Decision(Domain::IntegerNonnegative, "ProductionOT", setMonths);
    //Number of bicycles to be stored for each month
    store = gcnew Decision(Domain::IntegerNonnegative, "store", setMonths);

    model->AddDecisions(productionNormal, productionOT, store);

    //Demand forecast for each month
    pDemands = gcnew Parameter(Domain::IntegerNonnegative, "DemandForecast", setMonths);
    pDemands->SetBinding(DemandForecast, "Value", "Index");

    model->AddParameter(pDemands);
    //Cannot produce more than certain number of bicycles in normal hours
    model->AddConstraint("Caps", Model::ForEach(setMonths, gcnew Func<Term^, Term^>(this, &BicycleProduction::ProductionLimit)));

    //Cannot produce more than certain number of bicycles in over time
    model->AddConstraint("OTCaps", Model::ForEach(setMonths, gcnew Func<Term^, Term^>(this, &BicycleProduction::ProductionLimitOT)));

    //Stock of this month is the sum of stock in the previous month and supply of current month
    //For january, use the initial stock
    model->AddConstraint("SupplyDemand", Model::ForEach(setMonths, gcnew Func<Term^, Term^>(this, &BicycleProduction::SupplyDemand)));

    //Minimize the cost
    model->AddGoal("Cost", GoalKind::Minimize, Model::Sum(Model::ForEach(setMonths, gcnew Func<Term^, Term^>(this, &BicycleProduction::CostForMonth))));


    SimplexDirective^ simplex = gcnew SimplexDirective();
    //Directive^ simplex = gcnew Directive();

    Solution^ solution = context->Solve(simplex);
    Report^ report = solution->GetReport();
    Console::WriteLine(report);


	

    System::IO::StreamWriter^ sw;
    try{
      sw = gcnew System::IO::StreamWriter("Bicycle.mps");
      context->SaveModel(FileFormat::MPS, sw);
    }
    finally{
      if (sw)
        sw->Close();
    }
  }
  private:

  Term^ ProductionLimit(Term^ month){
    return (productionNormal[month] <= (double)iNormalProdCap);
  }
  Term^ ProductionLimitOT(Term^ month){
    return (productionOT[month] <= (double)iOTProdCap);
  }
  Term^ SupplyDemand(Term^ month){
    Term^ constraint = Model::Implies(month == (double)1, Model::Sum(productionNormal[month], productionOT[month], (double)iCurrentStock) == Model::Sum(pDemands[month], store[month]));
    Term^ constraintComplementary = Model::Implies(month != (double)1, Model::Sum(productionNormal[month], productionOT[month], store[month - 1.0]) == Model::Sum(pDemands[month], store[month]));
    return Model::And(constraint, constraintComplementary);
  }
  Term^ CostForMonth(Term^ month){
    return (productionNormal[month] * dCostNormal + productionOT[month] * dCostOt + store[month] * dCostStock);
  }

 
};


int main(array<System::String ^> ^args)
{
	int set_value = 12;
	int g_test_value[20];

	g_test_value[0] = 30000;
	g_test_value[1] = 15000;
	g_test_value[2] = 15000;
	g_test_value[3] = 25000;
	g_test_value[4] = 33000;
	g_test_value[5] = 40000;
	g_test_value[6] = 45000;
	g_test_value[7] = 45000;
	g_test_value[8] = 26000;
	g_test_value[9] = 14000;
	g_test_value[10] =25000;
	g_test_value[11] =30000;


	BicycleProduction^ bicycleProduction = gcnew BicycleProduction();
	bicycleProduction->inputData(set_value);
	bicycleProduction->Solve();
	return 0;
}
