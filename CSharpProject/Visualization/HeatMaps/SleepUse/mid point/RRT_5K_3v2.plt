set title "RRT 5K - 3 players vs. 2 enemies"
unset key
set tic scale 0
set palette rgbformula -7,2,-7
set cbrange [0:1]
set cblabel "Probability of Using Sleep"
unset cbtics
set xlabel "health"
set ylabel "attack"
set xrange [5.5:20.5]
set yrange [0.5:11.5]
set xtics 1
set ytics 1
plot '-' using 1:2:3 with image
6 1 0 1
6 2 0 1
6 3 0 1
6 4 0 1
6 5 0 1
6 6 0 1
6 7 0 1
6 8 0.2 0.8
6 9 0 1
6 10 0 1
6 11 0 1
7 1 0 1
7 2 0 1
7 3 0 1
7 4 0 1
7 5 0.1 0.8
7 6 0.2 0.7
7 7 0 1
7 8 0.1 0.8
7 9 0.3 0.7
7 10 0.2 0.6
7 11 0.2 0.8
8 1 0 1
8 2 0 0.9
8 3 0 0.9
8 4 0 0.3
8 5 0 0.3
8 6 0 0.2
8 7 0.2 0.5
8 8 0.1 0.3
8 9 0.2 0.4
8 10 0.2 0.3
8 11 0.1 0.2
9 1 0 1
9 2 0 1
9 3 0.8 0.2
9 4 0.6 0.4
9 5 0.7 0.3
9 6 0.2 0.8
9 7 0.2 0.8
9 8 0.3 0.7
9 9 0.3 0.7
9 10 0.2 0.8
9 11 0.1 0.9
10 1 0 1
10 2 0.1 0.9
10 3 0.3 0.7
10 4 0.6 0.4
10 5 0.9 0.1
10 6 0.5 0.5
10 7 0.6 0.4
10 8 0.7 0.3
10 9 0.3 0.7
10 10 0.4 0.6
10 11 0.6 0.4
11 1 0 1
11 2 0 1
11 3 0.2 0.8
11 4 0.5 0.5
11 5 0.7 0.3
11 6 0.3 0.7
11 7 0.3 0.7
11 8 0.1 0.9
11 9 0.5 0.5
11 10 0.5 0.5
11 11 0.4 0.6
12 1 0.1 0.9
12 2 0 1
12 3 0.5 0.5
12 4 0.3 0.7
12 5 0.6 0.4
12 6 0.3 0.7
12 7 0.2 0.8
12 8 0.2 0.8
12 9 0.2 0.8
12 10 0.5 0.5
12 11 0.9 0.1
13 1 0 1
13 2 0.2 0.8
13 3 0.2 0.8
13 4 0.6 0.4
13 5 0.6 0.4
13 6 0.4 0.6
13 7 0.2 0.8
13 8 0.7 0.3
13 9 0.8 0.2
13 10 0.6 0.4
13 11 0.5 0.5
14 1 0 1
14 2 0 1
14 3 0.2 0.8
14 4 0.3 0.7
14 5 0.5 0.5
14 6 0.7 0.3
14 7 0.7 0.3
14 8 0.5 0.5
14 9 0.7 0.3
14 10 0.8 0.2
14 11 0.5 0.4
15 1 0 1
15 2 0.2 0.8
15 3 0.6 0.2
15 4 0.5 0.1
15 5 0.4 0.3
15 6 0.3 0.2
15 7 0.3 0.4
15 8 0.3 0.3
15 9 0.5 0.2
15 10 0.3 0.4
15 11 0.1 0.8
16 1 0 1
16 2 0.3 0.7
16 3 1 0
16 4 0.8 0.2
16 5 0.8 0.2
16 6 0.8 0.2
16 7 0.1 0.9
16 8 0.7 0.3
16 9 0.5 0.5
16 10 0.7 0.3
16 11 0.7 0.3
17 1 0 1
17 2 0.1 0.9
17 3 0.8 0.2
17 4 0.7 0.3
17 5 0.7 0.3
17 6 0.6 0.4
17 7 0.3 0.7
17 8 0.7 0.3
17 9 0.6 0.4
17 10 0.2 0.8
17 11 0.5 0.4
18 1 0 1
18 2 0 1
18 3 0.7 0.3
18 4 1 0
18 5 1 0
18 6 0.6 0.4
18 7 0.2 0.8
18 8 0.5 0.5
18 9 0.5 0.5
18 10 0.5 0.5
18 11 0.7 0.2
19 1 0 1
19 2 0 1
19 3 0.7 0.3
19 4 0.7 0.3
19 5 0.5 0.5
19 6 0.6 0.4
19 7 0.3 0.7
19 8 0.6 0.4
19 9 0.4 0.6
19 10 0.9 0.1
19 11 0.6 0.4
20 1 0 1
20 2 0 1
20 3 0.8 0.2
20 4 0.8 0.1
20 5 0.5 0.5
20 6 0.7 0.3
20 7 0.4 0.6
20 8 0.5 0.5
20 9 0.3 0.7
20 10 0.5 0.5
20 11 0.5 0.5
e
