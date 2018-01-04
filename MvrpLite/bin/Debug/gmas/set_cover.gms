$ontext

    set cover problem
    junhua Chen.   cjh@bjtu.edu.cn
    4/4,2016

$offtext

set g 'group'/1*5/;
set p 'passenger'/1*21/;

parameter c(g,p)/
1.1 -100
1.2 -100
1.3 -100
1.4 -100
1.5 -100
1.6 -100
1.7 -100
1.8 -100
1.9 -100
1.10 -100
1.11 -100
1.12 4
1.13 4
1.14 4
1.15 4
1.16 4
1.17 4
1.18 4
1.19 4
1.20 4
1.21 4
2.1 2
2.2 2
2.3 2
2.4 2
2.5 2
2.6 2
2.7 2
2.8 2
2.9 2
2.10 2
2.11 2
2.12 -100
2.13 -100
2.14 -100
2.15 -100
2.16 -100
2.17 -100
2.18 -100
2.19 -100
2.20 -100
2.21 -100
3.1 2
3.2 2
3.3 2
3.4 2
3.5 2
3.6 2
3.7 2
3.8 2
3.9 2
3.10 2
3.11 2
3.12 -100
3.13 -100
3.14 -100
3.15 -100
3.16 -100
3.17 -100
3.18 -100
3.19 -100
3.20 -100
3.21 -100
4.1 -100
4.2 -100
4.3 -100
4.4 -100
4.5 -100
4.6 -100
4.7 2
4.8 2
4.9 2
4.10 2
4.11 2
4.12 -100
4.13 -100
4.14 -100
4.15 -100
4.16 -100
4.17 -100
4.18 -100
4.19 -100
4.20 -100
4.21 -100
5.1 -100
5.2 -100
5.3 -100
5.4 -100
5.5 -100
5.6 -100
5.7 -100
5.8 -100
5.9 -100
5.10 -100
5.11 -100
5.12 4
5.13 4
5.14 4
5.15 4
5.16 4
5.17 4
5.18 4
5.19 4
5.20 4
5.21 4
/;
scalar min_p_in_group/1/;
scalar max_p_in_group/8/;

binary variable x(g,p);
variable z;

equations
         obj_main
         assign_cover(p)
         low_capacity(g)
         up_capacity(g)
;
obj_main..
         z=e=sum((g,p),c(g,p)*x(g,p));
assign_cover(p)..
         sum(g,x(g,p))=g=1;
low_capacity(g)..
         sum(p,x(g,p))=g=min_p_in_group;
up_capacity(g)..
         sum(p,x(g,p))=l=max_p_in_group;


Model model_assignment /obj_main,assign_cover,low_capacity,up_capacity/;

solve model_assignment using mip maximizing z;
display x.l;
display z.l;

