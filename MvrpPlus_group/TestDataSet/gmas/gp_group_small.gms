$ontext

    vehicle and passenger partition problem
    junhua Chen.   cjh@bjtu.edu.cn
    4/4,2016

$offtext

set v 'group'/1*5/;
set p 'passenger'/1*8/;
alias (v,g);

parameter d(v,p);

parameter d(v,p)/
1.1 100
1.2 100
1.3 100
1.4 100
1.5 2
1.6 2
1.7 2
1.8 2
2.1 100
2.2 100
2.3 100
2.4 100
2.5 2
2.6 2
2.7 2
2.8 2
3.1 2
3.2 2
3.3 2
3.4 2
3.5 100
3.6 100
3.7 100
3.8 100
4.1 2
4.2 2
4.3 2
4.4 2
4.5 100
4.6 100
4.7 100
4.8 100
5.1 100
5.2 100
5.3 100
5.4 100
5.5 4
5.6 4
5.7 4
5.8 4
/;

parameter c(v,g);
c(v,g)=(sum(p,(d(v,p)-d(g,p))*(d(v,p)-d(g,p))));

parameter matrix_v_e_g(v,g);
          matrix_v_e_g(v,v)=1;

scalar capacity_v/4/;
scalar max_g/2/;
scalar min_g/2/;

binary variable x(v,g);
binary variable y(g);
variable z;

equations
         obj_main
         v_in_group_constrain(v)
         g_for_v_constrain(g)
         capacity_constrain_1
         capacity_constrain_2
         xy_connect(g),
         xy_connect_2(g)
;
obj_main..
         z=e=sum((v,g),c(v,g)*x(v,g));
v_in_group_constrain(v)..
         sum(g,x(v,g))=e=1;
g_for_v_constrain(g)..
         sum(v,x(v,g))=l=3;
capacity_constrain_1..
         sum(g,y(g))=g=min_g;
capacity_constrain_2..
         sum(g,y(g))=l=max_g;
xy_connect(g)..
         sum(v,x(v,g))=g=y(g);
xy_connect_2(g)..
         sum(v,x(v,g))-y(g)*1000=l=0;
model model_group /obj_main,
         v_in_group_constrain,
         g_for_v_constrain,
         capacity_constrain_1,
         capacity_constrain_2,
         xy_connect,
         xy_connect_2

         /;

solve model_group minimizing z using minlp;
display x.l;
display y.l;
display z.l;
