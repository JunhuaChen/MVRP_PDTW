$ontext

    vehicle and passenger partition problem
    junhua Chen.   cjh@bjtu.edu.cn
    4/4,2016

$offtext

set u 'vehicle'/1*5/;
alias (v,u);
alias (up,u);
set g 'group'/1*5/;
set p 'passenger'/1*8/;


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

parameter c(u,v);
c(u,v)=(sum(p,(d(u,p)-d(v,p))*(d(u,p)-d(v,p))));

parameter matrix_u_e_v(u,v);
          matrix_u_e_v(u,u)=1;

scalar max_g/2/;
scalar min_g/2/;

binary variable x(u,v,g);
variable z;

equations
         obj_main
         uv_in_group_constrain(u,v),
         constraint_1(u,v,g),
         constraint_2(g,u,v,up),
         constraint_3(g,v)

;
obj_main..
         z=e=sum((u,v,g),c(u,v)*x(u,v,g));
uv_in_group_constrain(u,v)..
         sum(g,x(u,v,g))=e=1;
constraint_1(u,v,g)..
         x(u,v,g)=e=x(v,u,g);
constraint_2(g,u,v,up)..
         x(u,v,g)+1000*(2-x(u,up,g)-x(up,v,g))=g=1;
constraint_3(g,u)..
         sum(v,x(u,v,g))=l=4;

model model_group /all

         /;

solve model_group minimizing z using minlp;
display x.l;

display z.l;
