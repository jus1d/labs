import tkinter as tk
import math

def apply_rules(axiom, rules):
    return ''.join([rules.get(c, c) for c in axiom])

def draw_sierpinski_carpet(axiom, length, angle, x, y, canvas):
    canvas.delete("all")
    for symbol in axiom:
        if symbol == 'F':
            x += length * math.cos(angle)
            y += length * math.sin(angle)
            canvas.create_line(x - length * math.cos(angle), y - length * math.sin(angle), x, y)
        elif symbol == '+':
            angle += math.pi / 3
        elif symbol == '-':
            angle -= math.pi / 3

def main():
    root = tk.Tk()
    root.title("Ковёр Серпинского")

    width = 800
    height = 800

    canvas = tk.Canvas(root, width=width, height=height, bg="white")
    canvas.pack()

    axiom = 'FXF--FF--FF'
    rules = {
        'F': 'FF',
        'X': '--FXF++FXF++FXF--',
    }
    iterations = 5
    length = 600
    angle = 0

    x, y = width // 10, height // 10

    current_iteration = [0]

    def generate_axiom():
        current_axiom = axiom
        for _ in range(current_iteration[0]):
            current_axiom = apply_rules(current_axiom, rules)
        return current_axiom

    def draw():
        current_axiom = generate_axiom()
        draw_sierpinski_carpet(current_axiom, length / 3 ** current_iteration[0], angle, x, y, canvas)

    def zoom(event):
        nonlocal length
        if event.delta > 0:
            length *= 1.1
        elif event.delta < 0:
            length /= 1.1
        draw()

    def increase_iteration(event):
        nonlocal current_iteration
        if current_iteration[0] < iterations:
            current_iteration[0] += 1
            draw()

    def decrease_iteration(event):
        nonlocal current_iteration
        if current_iteration[0] > 0:
            current_iteration[0] -= 1
            draw()

    is_dragging = False
    drag_start_x = 0
    drag_start_y = 0

    def start_drag(event):
        nonlocal is_dragging, drag_start_x, drag_start_y
        is_dragging = True
        drag_start_x = event.x
        drag_start_y = event.y

    def do_drag(event):
        nonlocal x, y, drag_start_x, drag_start_y
        if is_dragging:
            dx = event.x - drag_start_x
            dy = event.y - drag_start_y
            x += dx
            y += dy
            drag_start_x = event.x
            drag_start_y = event.y
            draw()

    def stop_drag(event):
        nonlocal is_dragging
        is_dragging = False

    canvas.bind("<Button-1>", start_drag)
    canvas.bind("<B1-Motion>", do_drag)
    canvas.bind("<ButtonRelease-1>", stop_drag)

    root.bind("<Up>", increase_iteration)
    root.bind("<Down>", decrease_iteration)

    canvas.bind("<MouseWheel>", zoom)

    draw()
    root.mainloop()

main()
