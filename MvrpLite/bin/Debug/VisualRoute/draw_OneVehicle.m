function draw_OneVehicle(dataRoute,dataNode,k,g_depot_No,g_vehicle_num)

    [mm,nn]=size(dataRoute);
    nCount=1;
    for i=1:mm
        if dataRoute(i,3)==k
            dataOneVehicle(nCount,1:8)=dataRoute(i,1:8);          
            nCount=nCount+1;
        end
    end
    if nCount==1
        return;
    end
    
    [mm_one,nn_one]=size(dataOneVehicle);
    if mm_one<=1
        return;
    end
    
     nCount=1;
    trainRoute(nCount,1)=g_depot_No;
    for i=1:mm_one-1
        if dataOneVehicle(i,6)<=61
            nCount=nCount+1;
            trainRoute(nCount,1)=dataOneVehicle(i,6);
        end    
    end
    
    %%%%%%%%%%%%%%%%%%%%%draw routes
     [mm,nn]=size(trainRoute);
    
     for i=1:mm-1
         x1=dataNode(find(dataNode(:,1)==trainRoute(i,1)),7);
         y1=dataNode(find(dataNode(:,1)==trainRoute(i,1)),8);
         
         x2=dataNode(find(dataNode(:,1)==trainRoute(i+1,1)),7);
         y2=dataNode(find(dataNode(:,1)==trainRoute(i+1,1)),8);
         
         %%%%%%draw line
        X = [x1;x2];
        Y = [y1;y2];
%         if(rem(k,5)==0)
%             plot(X,Y,'r','LineWidth',1);
%         elseif(rem(k,5)==1)
%             plot(X,Y,'b','LineWidth',1);
%         elseif(rem(k,5)==2)
%             plot(X,Y,'k','LineWidth',1);   
%         elseif(rem(k,5)==3)
%             plot(X,Y,'g','LineWidth',1);    
%           elseif(rem(k,5)==4)
%             plot(X,Y,'p','LineWidth',1);    
%         end  
         
        lineColor=jet(g_vehicle_num);
        plot(X,Y,'color',lineColor(k,:),'LineWidth',1);    
%          text(x1-.5,y1+1,strcat('', num2str(trainRoute(i,2))),'fontsize',8); 
         
         if i==mm-1
%               text(x2+.5,y2+.5,strcat('', num2str(trainRoute(i+1,1)))); 
         end
         
     end
    


end