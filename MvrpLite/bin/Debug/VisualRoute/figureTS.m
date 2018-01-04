 
clear;
clc;
close all;

path='..\';


g_depot_No=81;
dataNode=xlsread (strcat(path,'input_node.csv')); 
dataRoute=xlsread (strcat(path,'output_solution_route.csv')); 

g_vehicle_num=max(dataRoute(:,3));

%[mm,nn]=size(dataVehicle);


 %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%plot node
 
    hh=figure; hold on; grid on;
    [mm,nn]=size(dataNode);
    minX=100;maxX=-100;minY=100;maxY=0;
    for i=1:mm
       x=dataNode(i,7);
       y=dataNode(i,8);
       plot(x,y,'*');
       text(x+.3,y-1,strcat('n', num2str(dataNode(i,1))),'fontsize',8); 
       
       if x<=minX
           minX=x;       
       end
       if x>=maxX
           maxX=x;
       end
       if y<minY
           minY=y;
       end
       if y>maxY
           maxY=y;
       end
    end
    axis([minX-10,maxX+10,minY-10,maxY+10]); 
 
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
for k=1:g_vehicle_num
    
%    draw_OneVehicle(dataRoute,dataNode,k,g_depot_No,g_vehicle_num);
    
    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
    
    
end






