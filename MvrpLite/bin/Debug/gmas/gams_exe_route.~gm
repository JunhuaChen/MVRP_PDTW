$ontext

    set partition problem based on LR
    junhua Chen.   cjh@bjtu.edu.cn
    3/31,2016

$offtext

set g 'group';
set s 'states patter';
set p 'passenger';

parameter c(g,s);
parameter a(g,p,s);
**parameter initx(g,s);

*input data
$include "../gams_route_input.txt";

*--------------------------------------------------------------------
* standard MIP problem formulation
*--------------------------------------------------------------------
binary variable x(g,s);
variable z;

equations
         obj_main
         assign_pattern(g)
         assign_passenger(p)
;
obj_main..
         z=e=sum((g,s),c(g,s)*x(g,s));
assign_pattern(g)..
         sum(s,x(g,s))=e=1;
assign_passenger(p)..
         sum((g,s),a(g,p,s)*x(g,s))=e=1;

option optcr=0;
model model_main /obj_main,assign_pattern,assign_passenger/;
solve model_main using rmip minimizing z;



****out put
File output_results_x  /'../gams_route_output_x.txt'/;
put output_results_x;
loop((g,s)$(x.l(g,s)=1), put @1, g.tl, @5,s.tl, @10,x.l(g,s)/);
