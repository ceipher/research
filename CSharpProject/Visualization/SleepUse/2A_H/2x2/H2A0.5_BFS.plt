set title "BFS - (H2A0.5,H2A0.5) vs (X,H1A1)"
unset key
set tic scale 0
set palette rgbformula -7,2,-7
set cbrange [-0.1:1]
set cblabel "Probability of Using Sleep"
unset cbtics
set xlabel "health"
set ylabel "attack"
set xrange [2:37]
set yrange [1:25]
set xtics 3
set ytics 2
plot '-' using 1:2:3 with image
3 2 0
3 4 0
3 6 0
3 8 0
3 10 0
3 12 0
3 14 0
3 16 0
3 18 0
3 20 0
3 22 0
3 24 0
6 2 0
6 4 0
6 6 0
6 8 0.2
6 10 0.5
6 12 0.5
6 14 0.5
6 16 0.5
6 18 0.5
6 20 0.5
6 22 0.5
6 24 0.5
9 2 0
9 4 0.1428571
9 6 0.5
9 8 0.5
9 10 0.5
9 12 0.5
9 14 0.5
9 16 0.952381
9 18 0.952381
9 20 0.952381
9 22 0.952381
9 24 1
12 2 0
12 4 0.1428571
12 6 0.5
12 8 0.5
12 10 0.5
12 12 0.5
12 14 0.5
12 16 0.952381
12 18 0.952381
12 20 0.952381
12 22 0.952381
12 24 1
15 2 0
15 4 0.1666667
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
18 2 0
18 4 0.375
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
21 2 0
21 4 0.375
21 6 1
21 8 1
21 10 1
21 12 1
21 14 1
21 16 -0.1
21 18 -0.1
21 20 -0.1
21 22 -0.1
21 24 -0.1
24 2 0
24 4 0.375
24 6 1
24 8 1
24 10 1
24 12 1
24 14 1
24 16 -0.1
24 18 -0.1
24 20 -0.1
24 22 -0.1
24 24 -0.1
27 2 0
27 4 0.5
27 6 1
27 8 1
27 10 1
27 12 -0.1
27 14 -0.1
27 16 -0.1
27 18 -0.1
27 20 -0.1
27 22 -0.1
27 24 -0.1
30 2 0
30 4 0.5
30 6 1
30 8 -0.1
30 10 -0.1
30 12 -0.1
30 14 -0.1
30 16 -0.1
30 18 -0.1
30 20 -0.1
30 22 -0.1
30 24 -0.1
33 2 0
33 4 0.5
33 6 1
33 8 -0.1
33 10 -0.1
33 12 -0.1
33 14 -0.1
33 16 -0.1
33 18 -0.1
33 20 -0.1
33 22 -0.1
33 24 -0.1
36 2 0
36 4 0.5
36 6 1
36 8 -0.1
36 10 -0.1
36 12 -0.1
36 14 -0.1
36 16 -0.1
36 18 -0.1
36 20 -0.1
36 22 -0.1
36 24 -0.1
e
