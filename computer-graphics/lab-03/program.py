import tkinter as tk
import math

def apply_rules(axiom, rules):
    return ''.join([rules.get(c, c) for c in axiom])

def draw_sierpinski_triangle(axiom, length, angle, x, y, canvas):
    canvas.delete("all")
    stack = []
    current_direction = angle
    current_x, current_y = x, y

    for symbol in axiom:
        if symbol == 'F':
            new_x = current_x + length * math.cos(current_direction)
            new_y = current_y + length * math.sin(current_direction)
            canvas.create_line(current_x, current_y, new_x, new_y, fill="black")
            current_x, current_y = new_x, new_y
        elif symbol == '+':
            current_direction += math.radians(60)
        elif symbol == '-':
            current_direction -= math.radians(60)
        elif symbol == '[':
            stack.append((current_x, current_y, current_direction))
        elif symbol == ']':
            current_x, current_y, current_direction = stack.pop()

def main():
    root = tk.Tk()
    root.title("Треугольник Серпинского")

    width = 800
    height = 800

    canvas = tk.Canvas(root, width=width, height=height, bg="white")
    canvas.pack()

    axiom = "FXF--FF--FF"
    rules = {
        "F": "FF",
        "X": "--FXF++FXF++FXF--",
    }
    max_iterations = 6
    current_iteration = [0]

    angle = 0
    start_x = width // 10
    start_y = height * 8 // 10

    base_length = 300

    def generate_axiom():
        result = axiom
        for _ in range(current_iteration[0]):
            result = apply_rules(result, rules)
        return result

    def draw():
        current_axiom = generate_axiom()

        scale_factor = 0.5 ** current_iteration[0]
        current_length = base_length * scale_factor

        draw_sierpinski_triangle(current_axiom, current_length, angle, start_x, start_y, canvas)

    def zoom(event):
        nonlocal base_length
        if event.delta > 0:
            base_length *= 1.1
        elif event.delta < 0:
            base_length /= 1.1
        draw()

    def increase_iteration(event):
        if current_iteration[0] < max_iterations:
            current_iteration[0] += 1
            draw()

    def decrease_iteration(event):
        if current_iteration[0] > 0:
            current_iteration[0] -= 1
            draw()

    is_dragging = False
    drag_start_x = 0
    drag_start_y = 0
    offset = [start_x, start_y]

    def start_drag(event):
        nonlocal is_dragging, drag_start_x, drag_start_y
        is_dragging = True
        drag_start_x = event.x
        drag_start_y = event.y

    def do_drag(event):
        nonlocal offset, drag_start_x, drag_start_y, start_x, start_y
        if is_dragging:
            dx = event.x - drag_start_x
            dy = event.y - drag_start_y
            offset[0] += dx
            offset[1] += dy
            start_x, start_y = offset
            drag_start_x = event.x
            drag_start_y = event.y
            draw()

    def stop_drag(event):
        nonlocal is_dragging
        is_dragging = False

    canvas.bind("<Button-1>", start_drag)
    canvas.bind("<B1-Motion>", do_drag)
    canvas.bind("<ButtonRelease-1>", stop_drag)

    root.bind("k", increase_iteration)
    root.bind("j", decrease_iteration)

    canvas.bind("<MouseWheel>", zoom)
    canvas.bind("<Button-4>", lambda event: zoom(event))
    canvas.bind("<Button-5>", lambda event: zoom(event))

    draw()
    root.mainloop()

if __name__ == '__main__':
    main()
