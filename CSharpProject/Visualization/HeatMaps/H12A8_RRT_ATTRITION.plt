
set title "H=12 A=8\nWin Rate Using RRT (ATTRITION)"
unset key
set tic scale 0
set palette rgbformula -7,2,-7
set cbrange [0:1]
set cblabel "Probablity of Sleep on Enemy X"
unset cbtics
set xlabel "health"
set ylabel "attack"
set xrange [2:37]
set yrange [1:25]
set xtics 3
set ytics 2
plot '-' using 1:2:3 with image
3 2 1
3 4 1
3 6 1
3 8 1
3 10 1
3 12 1
3 14 1
3 16 1
3 18 1
3 20 1
3 22 1
3 24 1
6 2 1
6 4 1
6 6 1
6 8 1
6 10 1
6 12 1
6 14 1
6 16 1
6 18 1
6 20 1
6 22 1
6 24 1
9 2 1
9 4 1
9 6 1
9 8 1
9 10 1
9 12 1
9 14 1
9 16 1
9 18 1
9 20 1
9 22 1
9 24 1
12 2 1
12 4 1
12 6 1
12 8 1
12 10 1
12 12 1
12 14 1
12 16 1
12 18 1
12 20 1
12 22 1
12 24 1
15 2 1
15 4 1
15 6 1
15 8 1
15 10 1
15 12 1
15 14 1
15 16 1
15 18 1
15 20 1
15 22 1
15 24 1
18 2 1
18 4 1
18 6 1
18 8 1
18 10 1
18 12 1
18 14 1
18 16 1
18 18 1
18 20 1
18 22 1
18 24 1
21 2 1
21 4 1
21 6 1
21 8 1
21 10 1
21 12 1
21 14 1
21 16 1
21 18 1
21 20 1
21 22 1
21 24 1
24 2 1
24 4 1
24 6 1
24 8 1
24 10 1
24 12 1
24 14 1
24 16 1
24 18 1
24 20 1
24 22 1
24 24 1
27 2 1
27 4 1
27 6 1
27 8 1
27 10 1
27 12 1
27 14 1
27 16 1
27 18 1
27 20 1
27 22 1
27 24 1
30 2 1
30 4 1
30 6 1
30 8 1
30 10 1
30 12 1
30 14 1
30 16 1
30 18 1
30 20 1
30 22 1
30 24 1
33 2 1
33 4 1
33 6 1
33 8 1
33 10 1
33 12 0
33 14 0
33 16 0
33 18 0
33 20 0
33 22 0
33 24 0
36 2 1
36 4 1
36 6 1
36 8 1
36 10 1
36 12 0
36 14 0
36 16 0
36 18 0
36 20 0
36 22 0
36 24 0
e
