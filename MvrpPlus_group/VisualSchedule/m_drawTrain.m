function m_drawTrain(dataNode,dataTrainPlan,dataTrain,timeCoefficient,spaceCoefficient)

    trainIndex=0;
    for i=1:size(dataTrainPlan)
        trainNumber=dataTrainPlan(i,3);
        trainDirection=0;
        if dataTrainPlan(i,4)<dataTrainPlan(i,5)
            trainDirection=1;
        end

        for k=1:trainNumber
           trainCount=k+trainIndex;
           dataNowTrain=dataTrain(find(dataTrain(:,1)==trainCount),:);

           if trainDirection==0
               dataNowTrain=flipud(dataNowTrain);
           end
           
           %%%%%%%%start to draw one train
           
           for ii=1:size(dataNowTrain)
               nodeA_t=dataNowTrain(ii,4);
               nodeA_s=dataNode(dataNowTrain(ii,2),3)*spaceCoefficient;
               nodeB_t=dataNowTrain(ii,5);
               nodeB_s=dataNode(dataNowTrain(ii,3),3)*spaceCoefficient;
               
               % draw node
               plot(nodeA_t,nodeA_s,'k.');
               plot(nodeB_t,nodeB_s,'k.');
               %%%draw line
               T = [nodeA_t;nodeB_t];
               S = [nodeA_s;nodeB_s];
               plot(T,S,'b','LineWidth',1);
           end  
           
           %%%%%%%%%%%%%%%%%%end to draw one train
        end
        trainIndex=trainIndex+trainNumber;

    end

    ylabel( 'STATION');
    xlabel( 'TIME');
    %text(TS(1,3),TS(1,1),strcat('T', num2str(train)));  

    titleString=strcat('\bf Rail Timetable');
    titleString=strcat(titleString,'');
    title(titleString);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5

   
%     hold on;  %%grid on;
%     plot(TS(1,3),TS(1,1),'k.'); 
%     bNewStart=0;waitS=TS(1,3);waitT=TS(1,1);waitS_End=0;waitT_End=0;
%     
%     
%     if (mm>1) && (TS(mm,3)<TS(1,3))
%         direction=1;
%     else
%         direction=2;
%     end
%     for i=2:mm    
%        
%         %%%%%%draw line
%         T = [TS(i,1);TS(i-1,1)];
%         S = [TS(i,3);TS(i-1,3)];
%         plot(S,T,'b','LineWidth',1);
%         
%         %%%%%% draw arc
%         if S(1)==S(2)
%             if bNewStart==0
%                 waitT=T(2);waitS=S(2);
%             end
%             waitS_End=S(1);waitT_End=T(1);
%             bNewStart=1;
%         else
%             if bNewStart==1                             
%                 if waitT_End>waitT
%                     tSmall=waitT;tBig=waitT_End;
%                 else
%                     tSmall=waitT_End;tBig=waitT;
%                 end
%                  deltaY=tBig-tSmall;  
%                  if direction==1
%                      sx=[waitS,1.01*waitS,0.99*waitS,waitS];
%                  else
%                      sx=[waitS,0.99*waitS,1.01*waitS,waitS];
%                  end
%                 ty=[tSmall,tSmall+deltaY/3,tSmall+2*deltaY/3,tBig];         
% 
%                 values = spcrv([[sx(1) sx sx(end)];[ty(1) ty ty(end)]],3);
%                 plot(values(1,:),values(2,:),'b','LineWidth',1); 
%                 
%                 bNewStart=0;   
%             end
%         end   
%         
%          %%%draw node
%         if  bNewStart==0   
%             plot(TS(i,3),TS(i,1),'k.');
%         end
%     end
  
   
    
    

end
