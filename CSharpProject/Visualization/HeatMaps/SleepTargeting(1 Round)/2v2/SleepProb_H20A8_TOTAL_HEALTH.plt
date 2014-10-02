
set multiplot
set size 0.5,0.8
set origin 0.0,0.0
set title "H=20 A=8\nSleep Uses On Enemy X (TOTAL_HEALTH)"
unset key
set tic scale 0
set palette rgbformula -7,2,-7
set cbrange [-0.2:1]
set cblabel "Probablity of Sleep on Enemy X"
unset cbtics
set xlabel "health"
set ylabel "attack"
set xrange [3:62]
set yrange [1:25]
set xtics 5
set ytics 2
plot '-' using 1:2:3 with image
5 2 0
5 4 0
5 6 0
5 8 0
5 10 0
5 12 0
5 14 0
5 16 0
5 18 0
5 20 0
5 22 0
5 24 0
10 2 0
10 4 0
10 6 0
10 8 0
10 10 0
10 12 0
10 14 0
10 16 0
10 18 0
10 20 0
10 22 0
10 24 0
15 2 0
15 4 0
15 6 0
15 8 0
15 10 0
15 12 0
15 14 0
15 16 0
15 18 0
15 20 0
15 22 0
15 24 0
20 2 0
20 4 0
20 6 0
20 8 0.5
20 10 0.6
20 12 1
20 14 1
20 16 1
20 18 1
20 20 1
20 22 1
20 24 1
25 2 0
25 4 0
25 6 0
25 8 0.6666667
25 10 1
25 12 -0.2
25 14 -0.2
25 16 -0.2
25 18 -0.2
25 20 -0.2
25 22 -0.2
25 24 -0.2
30 2 0
30 4 0
30 6 0
30 8 0.6666667
30 10 1
30 12 -0.2
30 14 -0.2
30 16 -0.2
30 18 -0.2
30 20 -0.2
30 22 -0.2
30 24 -0.2
35 2 0
35 4 0
35 6 0
35 8 0.6666667
35 10 -0.2
35 12 -0.2
35 14 -0.2
35 16 -0.2
35 18 -0.2
35 20 -0.2
35 22 -0.2
35 24 -0.2
40 2 0
40 4 0
40 6 0
40 8 0.6666667
40 10 -0.2
40 12 -0.2
40 14 -0.2
40 16 -0.2
40 18 -0.2
40 20 -0.2
40 22 -0.2
40 24 -0.2
45 2 0
45 4 0
45 6 0
45 8 -0.2
45 10 -0.2
45 12 -0.2
45 14 -0.2
45 16 -0.2
45 18 -0.2
45 20 -0.2
45 22 -0.2
45 24 -0.2
50 2 0
50 4 0
50 6 0
50 8 -0.2
50 10 -0.2
50 12 -0.2
50 14 -0.2
50 16 -0.2
50 18 -0.2
50 20 -0.2
50 22 -0.2
50 24 -0.2
55 2 0
55 4 0
55 6 0
55 8 -0.2
55 10 -0.2
55 12 -0.2
55 14 -0.2
55 16 -0.2
55 18 -0.2
55 20 -0.2
55 22 -0.2
55 24 -0.2
60 2 0
60 4 0
60 6 0
60 8 -0.2
60 10 -0.2
60 12 -0.2
60 14 -0.2
60 16 -0.2
60 18 -0.2
60 20 -0.2
60 22 -0.2
60 24 -0.2
e
set origin 0.5,0.0 
set title "H=20 A=8\nSleep Uses On Enemy Team (TOTAL_HEALTH)"
set cblabel "Probablity of Sleep on Enemy Team"
plot '-' using 1:2:3 with image
5 2 1
5 4 0
5 6 0
5 8 0
5 10 0
5 12 0
5 14 0
5 16 0
5 18 0
5 20 0
5 22 0
5 24 0
10 2 1
10 4 1
10 6 1
10 8 1
10 10 1
10 12 1
10 14 1
10 16 1
10 18 1
10 20 1
10 22 1
10 24 1
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
20 2 1
20 4 1
20 6 1
20 8 1
20 10 1
20 12 1
20 14 1
20 16 1
20 18 1
20 20 1
20 22 1
20 24 1
25 2 1
25 4 1
25 6 1
25 8 1
25 10 1
25 12 -0.2
25 14 -0.2
25 16 -0.2
25 18 -0.2
25 20 -0.2
25 22 -0.2
25 24 -0.2
30 2 1
30 4 1
30 6 1
30 8 1
30 10 1
30 12 -0.2
30 14 -0.2
30 16 -0.2
30 18 -0.2
30 20 -0.2
30 22 -0.2
30 24 -0.2
35 2 1
35 4 1
35 6 1
35 8 1
35 10 -0.2
35 12 -0.2
35 14 -0.2
35 16 -0.2
35 18 -0.2
35 20 -0.2
35 22 -0.2
35 24 -0.2
40 2 1
40 4 1
40 6 1
40 8 1
40 10 -0.2
40 12 -0.2
40 14 -0.2
40 16 -0.2
40 18 -0.2
40 20 -0.2
40 22 -0.2
40 24 -0.2
45 2 1
45 4 1
45 6 1
45 8 -0.2
45 10 -0.2
45 12 -0.2
45 14 -0.2
45 16 -0.2
45 18 -0.2
45 20 -0.2
45 22 -0.2
45 24 -0.2
50 2 1
50 4 1
50 6 1
50 8 -0.2
50 10 -0.2
50 12 -0.2
50 14 -0.2
50 16 -0.2
50 18 -0.2
50 20 -0.2
50 22 -0.2
50 24 -0.2
55 2 1
55 4 1
55 6 1
55 8 -0.2
55 10 -0.2
55 12 -0.2
55 14 -0.2
55 16 -0.2
55 18 -0.2
55 20 -0.2
55 22 -0.2
55 24 -0.2
60 2 1
60 4 1
60 6 1
60 8 -0.2
60 10 -0.2
60 12 -0.2
60 14 -0.2
60 16 -0.2
60 18 -0.2
60 20 -0.2
60 22 -0.2
60 24 -0.2
e
unset multiplot
