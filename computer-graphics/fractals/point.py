import math

def cos(a: float) -> float:
    return math.cos(math.radians(a))

def sin(a: float) -> float:
    return math.sin(math.radians(a))

class Point:
    def __init__(self, x: float, y: float):
        self.x = x
        self.y = y

    def move(self, d: int, angle: int) -> 'Point':
        return Point(self.x + d * cos(angle), self.y + d * sin(angle))
