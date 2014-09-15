set title "RRT 5K - (H2A0.5,H2A0.5) vs (X,H1A1)"
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
6 8 0.9
6 10 0.5
6 12 0.6
6 14 1
6 16 0.7
6 18 1
6 20 0.7
6 22 1
6 24 0.6
9 2 0
9 4 0.5
9 6 0.7
9 8 0.6
9 10 0.2
9 12 0.5
9 14 0.2
9 16 0.3
9 18 0.7
9 20 0.7
9 22 1
9 24 1
12 2 0
12 4 0.5
12 6 0.7
12 8 0.8
12 10 0.4
12 12 0.6
12 14 0.6
12 16 0.3
12 18 0.6
12 20 1
12 22 0.9
12 24 1
15 2 0
15 4 0.6
15 6 0.7
15 8 1
15 10 0.7
15 12 1
15 14 1
15 16 1
15 18 1
15 20 1
15 22 1
15 24 1
18 2 0
18 4 0.8
18 6 1
18 8 -0.1
18 10 -0.1
18 12 1
18 14 1
18 16 -0.1
18 18 -0.1
18 20 -0.1
18 22 -0.1
18 24 -0.1
21 2 0
21 4 0.6
21 6 1
21 8 -0.1
21 10 -0.1
21 12 -0.1
21 14 -0.1
21 16 -0.1
21 18 -0.1
21 20 -0.1
21 22 -0.1
21 24 -0.1
24 2 0
24 4 0.7
24 6 1
24 8 -0.1
24 10 -0.1
24 12 1
24 14 -0.1
24 16 -0.1
24 18 -0.1
24 20 -0.1
24 22 -0.1
24 24 -0.1
27 2 0
27 4 0.6
27 6 1
27 8 -0.1
27 10 -0.1
27 12 -0.1
27 14 -0.1
27 16 -0.1
27 18 -0.1
27 20 -0.1
27 22 -0.1
27 24 -0.1
30 2 0
30 4 0.7
30 6 -0.1
30 8 -0.1
30 10 -0.1
30 12 -0.1
30 14 -0.1
30 16 -0.1
30 18 -0.1
30 20 -0.1
30 22 -0.1
30 24 -0.1
33 2 0.2
33 4 -0.1
33 6 -0.1
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
36 4 -0.1
36 6 -0.1
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