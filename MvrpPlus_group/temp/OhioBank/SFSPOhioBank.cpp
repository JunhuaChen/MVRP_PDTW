// Copyright ?Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Text;
using namespace Microsoft::SolverFoundation::Services;

ref class OhioBank  {

public:
  void static Run() {

    SolverContext^ context = SolverContext::GetContext();
    Model^ model = context->CreateModel();

    array< double >^ regular_pay; 
    regular_pay = gcnew array<double >(24); 
    regular_pay[11] = 90;    regular_pay[12] = 91;    regular_pay[13] = 92;

    array< double >^ overtime_pay; 
    overtime_pay = gcnew array<double >(24); 
    overtime_pay[11] = overtime_pay[12] = overtime_pay[13] = 18;

    array< double >^ parttime_pay; 
    parttime_pay = gcnew array<double >(24); 

    parttime_pay[11] = parttime_pay[12] = parttime_pay[13] = parttime_pay[14] = 28;
    parttime_pay[15] = 29;  parttime_pay[16] = 30; parttime_pay[17] = 31; parttime_pay[18] = 32;

    array< double >^ work_in; 
    work_in = gcnew array<double >(24); 
    work_in[11] = 10; work_in[12] = 11; work_in[13] = 15; work_in[14] = 20; work_in[15] = 25; work_in[16] = 28; 
    work_in[17] = 32; work_in[18] = 50; work_in[19] = 30; work_in[20] = 20; work_in[21] = 8;

    array< Decision^ >^ regular;
    regular = gcnew array<Decision^ >(24);
    // Starting hours of regular workers. Each worker works 4 hours, has a 1 hour break, then works another 4 hours.
    for (int i= 11; i <14 ; i ++){
      regular[i] = gcnew Decision(Domain::RealNonnegative, String::Format(L"regular_{0}", i));
      model->AddDecision(regular[i]);
    }
    // Number of regular workers who also work overtime. Each overtime worker works 1 hour after their regular shift
    array< Decision^ >^ overtime;
    overtime = gcnew array<Decision^ >(24);
    for (int i= 11; i <13 ; i ++){
      overtime[i] = gcnew Decision(Domain::RealNonnegative, String::Format(L"overtime_{0}", i));
      model->AddDecision(overtime[i]);
    }
    // Starting hours of part-time workers. Each worker works 4 hours.
    array< Decision^ >^ parttime;
    parttime = gcnew array<Decision^ >(24);
    for (int i= 11; i <19 ; i ++){
      parttime[i] = gcnew Decision(Domain::RealNonnegative, String::Format(L"parttime_{0}", i));
      model->AddDecision(parttime[i]);
    }
    // Work left over at the end of each hour
    array< Decision^ >^ backlog;
    backlog = gcnew array<Decision^ >(24);
    for (int i= 11; i <22 ; i ++){
      backlog[i] = gcnew Decision(Domain::RealNonnegative, String::Format(L"backlog_{0}", i));
      model->AddDecision(backlog[i]);
    }
    // Find the total number of workers working
    array< Term^ >^ regular_workers;
    regular_workers = gcnew array<Term^ >(24);
    array< Term^ >^ overtime_workers;
    overtime_workers = gcnew array<Term^ >(24);
    array< Term^ >^ parttime_workers;
    parttime_workers = gcnew array<Term^ >(24);
    for (int i= 11; i <22 ; i ++){
      regular_workers[i] = 0.0;
      for (int j = i; j>=Math::Max(11, i-8); j--){
        if (j != i-4 && j < 14)
          regular_workers[i] += regular[j];
      }
      overtime_workers[i] = 0.0;
      if (i - 9 >= 11)
        overtime_workers[i] = overtime[i-9];
      parttime_workers[i] = 0.0;
      for (int j = i; j>=Math::Max(11, i-3); j--)
        if (j < 19)
          parttime_workers[i] += parttime[j];
    }

    // There is a limit to the number of workers working at any given time
    for (int i= 11; i <22 ; i ++){
      model->AddConstraint(String::Format(L"workers_{0}", i), Model::LessEqual(regular_workers[i] + overtime_workers[i] + parttime_workers[i], 35.0));
    }
    // No more than half of workers can work overtime in any shift
    for (int i= 11; i <13 ; i ++){
      model->AddConstraint(String::Format(L"overtimelimit_{0}", i), Model::LessEqual(2.0 * overtime[i], regular[i]));
    }
    // No more than 20 total overtime hours are allowed
    model->AddConstraint(L"overtimetotal", Model::LessEqual(overtime[11] + overtime[12], 20.0));

    // The amount of work remaining at the end of each hour is the work at the start of the hour, plus the new work, minus the work done.
    Term^ current_progress = nullptr;
    Term^ current_capacity = nullptr;
    //special case for first hour (no backlog for the hour before
    current_progress = work_in[11] - backlog[11];
    current_capacity = regular_workers[11] + overtime_workers[11] + 0.8 * parttime_workers[11];
    model->AddConstraint(String::Format(L"work_{0}", 11), Model::GreaterEqual(current_capacity, current_progress));
    for (int i= 12; i <22 ; i ++){
      current_progress = backlog[i-1] + work_in[i] - backlog[i];
      current_capacity = regular_workers[i] + overtime_workers[i] + 0.8 * parttime_workers[i];
      model->AddConstraint(String::Format(L"work_{0}", i), Model::GreaterEqual(current_capacity, current_progress));
    }
    // There must be no work remaining after the last shift
    model->AddConstraint("finished", Model::Equal(backlog[21], 0.0));
    // There may be no more than 20 work units of backlog at any time
    for (int i= 11; i <22 ; i ++){
      model->AddConstraint(String::Format(L"maxbacklog_{0}", i), Model::LessEqual(backlog[i], 20.0));
    }

    // The goal is to minimize the total wages
    Term^ total_wages = 0.0;
    for (int i= 11; i <14 ; i ++)
      total_wages+= regular_pay[i] * regular[i];
    for (int i= 11; i <13 ; i ++)
      total_wages+= overtime_pay[i] * overtime[i];
    for (int i= 11; i <19 ; i ++)
      total_wages+= parttime_pay[i] * parttime[i];
    model->AddGoal("wages", GoalKind::Minimize, total_wages);


    Solution^ solution = context->Solve(gcnew SimplexDirective());
    Report^ report = solution->GetReport();
    Console::Write(L"{0}", report);
  }
};

int main(array<System::String ^> ^args)
{
  OhioBank::Run();
  return 0;
}
