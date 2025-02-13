#!/usr/bin/env python3

from point import Point
import tkinter as tk
import fractal

WIDTH = 800
HEIGHT = 600

LINE_COLOR = 'black'
LINE_WIDTH = 2

win = tk.Tk()
win.geometry('%dx%d' % (WIDTH, HEIGHT))
canvas = tk.Canvas(win, width=WIDTH, height=HEIGHT)
canvas.pack()

S = fractal.generate(7)

p = Point(200, 200)
angle = 0
for c in S:
    if c == 'F':
        pnext = p.move(fractal.LINE_LENGTH, angle)
        canvas.create_line(p.x, p.y, pnext.x, pnext.y, fill=LINE_COLOR, width=LINE_WIDTH)
        p = pnext
    elif c == '-':
        angle -= fractal.ALPHA
    elif c == '+':
        angle += fractal.ALPHA
    else:
        print(f'[DEBUG]: not used char: {c}')


win.mainloop()
