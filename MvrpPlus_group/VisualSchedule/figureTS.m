 
clear;
clc;
close all;

path='..\VRP_testing\';
[dataNode]=xlsread(strcat(path,'data_station.csv'));
[dataTrainPlan]=xlsread(strcat(path,'data_train_plan.csv'));
dataTrain=load (strcat(path,'gams_output_tad.txt')); 

hh=figure;hold on;


g_max_train=11;
g_time_coefficient=1;
g_space_coefficient=100;
 
m_drawAxis(dataNode,dataTrain,g_time_coefficient,g_space_coefficient);

m_drawTrain(dataNode,dataTrainPlan,dataTrain,g_time_coefficient,g_space_coefficient);

