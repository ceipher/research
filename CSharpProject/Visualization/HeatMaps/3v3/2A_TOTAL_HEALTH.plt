
set multiplot
set size 0.5,0.8
set origin 0.0,0.0
set title "Sleep Uses On Enemy X (TOTAL_HEALTH)"
unset key
set tic scale 0
set palette rgbformula -7,2,-7
set cbrange [-0.2:1]
set cblabel "Probablity of Sleep on Enemy X"
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
3 8 0.1333333
3 10 0.1538462
3 12 0.1538462
3 14 0.1538462
3 16 0.1538462
3 18 0.1538462
3 20 0.1538462
3 22 0.1538462
3 24 0.1538462
6 2 0
6 4 0
6 6 0
6 8 0.1333333
6 10 0.1538462
6 12 0.1538462
6 14 0.1538462
6 16 0.1538462
6 18 0.1538462
6 20 0.1538462
6 22 0.1538462
6 24 0.1538462
9 2 0
9 4 0
9 6 0
9 8 0.3333333
9 10 0.5
9 12 0.5
9 14 0.5
9 16 0.5
9 18 0.5
9 20 0.5
9 22 0.5
9 24 0.5
12 2 0
12 4 0
12 6 0
12 8 0.3333333
12 10 0.5
12 12 0.5
12 14 0.5
12 16 0.5
12 18 0.5
12 20 0.5
12 22 0.5
12 24 0.5
15 2 0
15 4 0
15 6 0
15 8 0.3333333
15 10 0.5
15 12 0.5
15 14 0.5
15 16 0.5
15 18 0.5
15 20 0.5
15 22 0.5
15 24 0.5
18 2 0
18 4 0
18 6 0
18 8 0.75
18 10 1
18 12 1
18 14 1
18 16 1
18 18 1
18 20 1
18 22 1
18 24 1
21 2 0
21 4 0
21 6 0
21 8 0.75
21 10 1
21 12 1
21 14 1
21 16 1
21 18 1
21 20 1
21 22 1
21 24 1
24 2 0
24 4 0
24 6 0
24 8 0.75
24 10 1
24 12 1
24 14 1
24 16 1
24 18 1
24 20 1
24 22 1
24 24 1
27 2 0
27 4 0.1666667
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
30 2 0
30 4 0.1666667
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
33 2 0
33 4 0.125
33 6 1
33 8 1
33 10 1
33 12 1
33 14 1
33 16 1
33 18 1
33 20 1
33 22 1
33 24 1
36 2 0
36 4 0.125
36 6 1
36 8 1
36 10 1
36 12 1
36 14 1
36 16 1
36 18 1
36 20 1
36 22 1
36 24 1
e
set origin 0.5,0.0 
set title "Sleep Uses On Enemy Team (TOTAL_HEALTH)"
set cblabel "Probablity of Sleep on Enemy Team"
plot '-' using 1:2:3 with image
3 2 1
3 4 1
3 6 1
3 8 0.9333333
3 10 0.9230769
3 12 0.9230769
3 14 0.9230769
3 16 0.9230769
3 18 0.9230769
3 20 0.9230769
3 22 0.9230769
3 24 0.9230769
6 2 1
6 4 1
6 6 1
6 8 0.9333333
6 10 0.9230769
6 12 0.9230769
6 14 0.9230769
6 16 0.9230769
6 18 0.9230769
6 20 0.9230769
6 22 0.9230769
6 24 0.9230769
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
33 12 1
33 14 1
33 16 1
33 18 1
33 20 1
33 22 1
33 24 1
36 2 1
36 4 1
36 6 1
36 8 1
36 10 1
36 12 1
36 14 1
36 16 1
36 18 1
36 20 1
36 22 1
36 24 1
e
unset multiplot
