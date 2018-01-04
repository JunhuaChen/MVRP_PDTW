function m_drawAxis(dataNode,dataTrain,timeCoefficient,spaceCoefficient)

   maxTimeInternal=10;
   minTimeInternal=2;
%%%%%%%%%%%%%%%%%%%%%%%%% range
   
    minT=min(dataTrain(:,4));maxT=max(dataTrain(:,5));
    minS=min(dataNode(:,3));maxS=max(dataNode(:,3));

    maxT=fix(maxT+1);
    maxS=fix(maxS+1)*100;
    minS=0;
    minT=0;
 %%%%%%%%%%%%%%%%%%%%%%%%%draw station 
%    while rem(maxT,2)~=0
%        maxT=maxT+1;
%    end
%     while rem(maxS,5)~=0
%        maxS=maxS+1;
%        minS=minS-1;
%    end
   
   %%%%draw  --
   V=dataNode(:,3)*spaceCoefficient;
   
   for i=1:size(V)
        plot([0;maxT],[V(i,1);V(i,1)],'g','LineWidth',2);
   end
   
   
   %%%%%%%%%draw |||
   max_space=V(1,1);   
   min_space=V(i,1);   
   k=1;
   for i=0:minTimeInternal:maxT/timeCoefficient
        if rem(i,maxTimeInternal)==0
            plot([i*timeCoefficient;i*timeCoefficient],[min_space;max_space],'g','LineWidth',2); 
            label(k)=i;
            k=k+1;
        else
            plot([i*timeCoefficient;i*timeCoefficient],[min_space;max_space],'g','LineWidth',1);   
        end
   end
   
   %%%%%%%deal with axis
   maxS=max_space-minS;
   axis([-4*2,maxT,minS,maxS+100]);   
      
   set(gca,'XTick',0:maxTimeInternal*timeCoefficient:maxT)
   set(gca,'XTickLabel',label);
   set(gca,'YTick',0:100:100)
   set(gca,'YTickLabel','');

end

