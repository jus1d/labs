from tkinter import ttk, colorchooser
import tkinter as tk
from typing import Optional

import sys
sys.setrecursionlimit(10_000)

CANVAS_WIDTH = 800
CANVAS_HEIGHT = 600

class Paint:
    def __init__(self, root: tk.Tk):
        self.root = root
        root.title("Lab. 03")

        self.mode = tk.StringVar(value="line_bresenham")
        self.current_color = "#000000"

        toolbar = ttk.Frame(root)
        toolbar.pack(side=tk.TOP, fill=tk.X)

        ttk.Label(toolbar, text="Line mode:").pack(side=tk.LEFT, padx=5)
        for text, mode in [("Default", "line_natural"),
                           ("Bresenham", "line_bresenham"),
                           ("Bezier", "line_bezier")]:
            ttk.Radiobutton(toolbar, text=text, variable=self.mode, value=mode).pack(side=tk.LEFT, padx=5)

        ttk.Label(toolbar, text="Fill mode:").pack(side=tk.LEFT, padx=15)
        for text, mode in [("Recursive", "fill_recursive"),
                           ("Koroyed", "fill_koroyed")]:
            ttk.Radiobutton(toolbar, text=text, variable=self.mode, value=mode).pack(side=tk.LEFT, padx=5)

        btn_color = ttk.Button(toolbar, text="Select color", command=self.choose_color)
        btn_color.pack(side=tk.LEFT, padx=5)

        self.image = tk.PhotoImage(width=CANVAS_WIDTH, height=CANVAS_HEIGHT)
        white_line = "{" + " ".join(["#ffffff"] * CANVAS_WIDTH) + "}"
        for y in range(CANVAS_HEIGHT):
            self.image.put(white_line, to=(0, y))

        self.canvas = tk.Canvas(root, width=CANVAS_WIDTH, height=CANVAS_HEIGHT)
        self.canvas.pack()
        self.canvas_image = self.canvas.create_image(0, 0, image=self.image, anchor=tk.NW)

        self.click_points = []

        self.canvas.bind("<Button-1>", self.on_click)

    def choose_color(self) -> None:
        color = colorchooser.askcolor()[1]
        if color:
            self.current_color = color

    def on_click(self, event) -> None:
        current_mode = self.mode.get()
        if current_mode.startswith("line"):
            if current_mode == "line_bezier":
                self.click_points.append((event.x, event.y))
                if len(self.click_points) == 3:
                    self.draw_bezier(self.click_points[0], self.click_points[1], self.click_points[2], self.current_color)
                    self.click_points = []
            else:
                self.click_points = [(event.x, event.y)]
                self.canvas.bind("<ButtonRelease-1>", self.on_release_line)
        elif current_mode.startswith("fill"):
            if current_mode == "fill_recursive":
                self.fill_recursive(event.x, event.y, self.current_color)
            elif current_mode == "fill_koroyed":
                self.fill_koroyed(event.x, event.y, self.current_color)

    def on_release_line(self, event) -> None:
        start = self.click_points[0]
        end = (event.x, event.y)

        if self.mode.get() == "line_bresenham":
            self.draw_line_bresenham(start[0], start[1], end[0], end[1], self.current_color)
        elif self.mode.get() == "line_natural":
            self.draw_line_natural(start[0], start[1], end[0], end[1], self.current_color)

        self.canvas.unbind("<ButtonRelease-1>")
        self.click_points = []

    def draw_line_natural(self, x0: int, y0: int, x1: int, y1: int, color: str) -> None:
        steps = int(max(abs(x1 - x0), abs(y1 - y0)))

        if steps == 0:
            self.set_pixel(x0, y0, color)
        else:
            for i in range(steps + 1):
                t = i / steps
                x = round(x0 + (x1 - x0) * t)
                y = round(y0 + (y1 - y0) * t)
                self.set_pixel(x, y, color)

        self.canvas.update()

    def draw_line_bresenham(self, x0: int, y0: int, x1: int, y1: int, color: str, update: bool = True):
        dx = abs(x1 - x0)
        dy = abs(y1 - y0)
        sx = 1 if x0 < x1 else -1
        sy = 1 if y0 < y1 else -1
        err = dx - dy

        while True:
            self.set_pixel(x0, y0, color)
            if x0 == x1 and y0 == y1:
                break
            e2 = 2 * err
            if e2 > -dy:
                err -= dy
                x0 += sx
            if e2 < dx:
                err += dx
                y0 += sy

        if update: self.canvas.update()

    def draw_bezier(self, p0: tuple[int, int], p1: tuple[int, int], p2: tuple[int, int], color: str):
        steps = 100
        prev_point = None

        for i in range(steps + 1):
            t = i / steps
            x = (1-t)**2 * p0[0] + 2*(1-t)*t * p1[0] + t**2 * p2[0]
            y = (1-t)**2 * p0[1] + 2*(1-t)*t * p1[1] + t**2 * p2[1]

            if prev_point:
                self.draw_line_bresenham(round(prev_point[0]), round(prev_point[1]), round(x), round(y), color, False)
            prev_point = (x, y)

        if not prev_point: return

        self.draw_line_bresenham(round(prev_point[0]), round(prev_point[1]), round(p2[0]), round(p2[1]), color, False)
        self.canvas.update()

    def fill_recursive(self, x: int, y: int, fill_color: str):
        def inner(x: int, y: int, target_color: str, fill_color: str) -> None:
            if x < 0 or x >= CANVAS_WIDTH or y < 0 or y >= CANVAS_HEIGHT:
                return

            current = self.get_pixel(x, y)
            if current != target_color or current == fill_color:
                return

            self.set_pixel(x, y, fill_color)
            inner(x+1, y, target_color, fill_color)
            inner(x-1, y, target_color, fill_color)
            inner(x, y+1, target_color, fill_color)
            inner(x, y-1, target_color, fill_color)

        target_color = self.get_pixel(x,y)

        if target_color == fill_color or not target_color:
            return

        inner(x, y, target_color, fill_color)
        self.canvas.update()

    def fill_koroyed(self, x: int, y: int, fill_color: str) -> None:
        target_color = self.get_pixel(x, y)
        if target_color == fill_color:
            return

        stack = [(x, y)]
        while stack:
            cx, cy = stack.pop()
            left = cx

            while left >= 0 and self.get_pixel(left, cy) == target_color:
                left -= 1

            left += 1
            right = cx

            while right < CANVAS_WIDTH and self.get_pixel(right, cy) == target_color:
                right += 1
            right -= 1

            for i in range(left, right+1):
                self.set_pixel(i, cy, fill_color)

                if cy > 0 and self.get_pixel(i, cy-1) == target_color:
                    stack.append((i, cy-1))
                if cy < CANVAS_HEIGHT-1 and self.get_pixel(i, cy+1) == target_color:
                    stack.append((i, cy+1))

        self.canvas.update()

    def set_pixel(self, x: int, y: int, color: str) -> None:
        if 0 <= x < CANVAS_WIDTH and 0 <= y < CANVAS_HEIGHT:
            self.image.put(color, (x, y))

    def get_pixel(self, x: int, y: int) -> Optional[str]:
        if 0 <= x < CANVAS_WIDTH and 0 <= y < CANVAS_HEIGHT:
            result = self.image.get(x, y)

            if isinstance(result, str):
                try:
                    r, g, b = map(int, result.split())
                except Exception:
                    r, g, b = (255, 255, 255)
            elif isinstance(result, tuple):
                r, g, b = result
            else:
                r, g, b = (255, 255, 255)
            return f'#{r:02x}{g:02x}{b:02x}'

        return None

if __name__ == "__main__":
    root = tk.Tk()
    app = Paint(root)
    root.mainloop()
