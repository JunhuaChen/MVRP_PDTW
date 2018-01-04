$ontext

    vehicle and passenger partition problem
    junhua Chen.   cjh@bjtu.edu.cn
    5/27,2016

$offtext

set u 'vehicle';
scalar M;
scalar Infinite;
alias(u,v);
parameter c(u,v);
parameter matrix_u_v(u,v);


binary variable x(u,v);
variable z;

equations
         obj_main,
         out_constrain(u),
         in_constrain(v)
;
obj_main..
         z=e=sum((u,v),c(u,v)*x(u,v));
out_constrain(u)..
         sum(v$(matrix_u_v(u,v)=0),x(u,v))=l=1;
in_constrain(v)..
         sum(u,x(u,v))=e=1;

model model_group / obj_main,
         out_constrain,
         in_constrain
         /;

*****input data

$include "..\gams_vehicle_connection_input.txt";
matrix_u_v(u,v)=0;
matrix_u_v(u,u)=1;
************

solve model_group minimizing z using MIP;
display x.l;
display z.l;

*************************************
**output

File output_results_x  /'..\gams_vehicle_connection_output.txt'/;
put output_results_x;
loop((u,v)$(x.l(u,v)=1), put @1, u.tl, @5,v.tl, @10, x.l(u,v)/);
