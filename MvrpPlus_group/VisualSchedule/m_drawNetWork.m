function  m_drawNetWork(dataNode,dataLink,netCoefficient)

    V=dataNode(:,5:6);
    V=[V,dataNode(:,3)];    
    k=1;
    for i=1:size(dataLink)
       if dataLink(i,7)>10  %%% link type choose.
           continue;
       else
           E(k,1:3)=dataLink(i,3:5);
           k=k+1;
       end
    end
    
    
    V(:,2)=netCoefficient*V(:,2);
    
    for i=1:size(E)      
       nodeA=V(find(V(:,3)==E(i,1)),1:2);
       nodeB=V(find(V(:,3)==E(i,2)),1:2);
       E_xy(i,1:2)=nodeA;
       E_xy(i,3:4)=nodeB;
    end
   
    %%%%%%%%%%%%%%%%%%%%start to draw
    hh=figure; hold on;

    plot(V(:,1),V(:,2),'k.','MarkerSize',15);%draw node

    for k=1:size(V),
     s=sprintf('%d',V(k,3)); 
     hhh=text(V(k,1)+0.05,V(k,2)-0.35*netCoefficient,s);
    end  
    %  set(hhh,'FontName','Times New Roman Cyr','FontSize',18)

    for k=1:size(E_xy)
        plot([E_xy(k,1),E_xy(k,3)],[E_xy(k,2),E_xy(k,4)],'k-','LineWidth',2);       
    end
end

