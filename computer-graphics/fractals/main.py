#!/usr/bin/env python3

from point import Point
import tkinter as tk
import fractal

WIDTH = 800
HEIGHT = 600

LINE_COLOR = 'black'
LINE_WIDTH = 2
scale = 1

win = tk.Tk()
win.geometry('%dx%d' % (WIDTH, HEIGHT))
canvas = tk.Canvas(win, width=WIDTH, height=HEIGHT)
canvas.pack()

def draw(n):
    global scale
    canvas.delete('all')
    S = fractal.generate(n)

    p = Point(200, 400)
    angle = 0
    for c in S:
        if c == 'F':
            pnext = p.move(int(fractal.line_length/scale), angle)
            canvas.create_line(p.x, p.y, pnext.x, pnext.y, fill=LINE_COLOR, width=LINE_WIDTH)
            p = pnext
        elif c == '-':
            angle -= fractal.ALPHA
        elif c == '+':
            angle += fractal.ALPHA
        else:
            # print(f'[DEBUG]: not used char: {c}')
            pass


def key_press(event):
    global scale
    # delta = 2
    if event.keysym == 'minus':
        scale = max(scale-1, 1)
    elif event.keysym == 'equal':
        scale = min(scale+1, 10)
    else:
        return

    draw(scale)

win.bind('<Key>', key_press)

win.mainloop()
