import pygame
import math
import numpy as np
from pygame.locals import QUIT, VIDEORESIZE, K_LEFT, K_RIGHT, K_UP, K_DOWN, K_q, K_e, K_w, K_s, K_a, K_d, K_j, K_k

pygame.init()

INIT_WIDTH, INIT_HEIGHT = 800, 600
screen = pygame.display.set_mode((INIT_WIDTH, INIT_HEIGHT), pygame.RESIZABLE)
pygame.display.set_caption("6201-020302D - Фадеев Артем Максимович")

WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
BLUE = (0, 0, 255)

R = 60
H = 120
N = 40
M = 15

zk = 500
zpp = 300

def projection_matrix(z, zk=zk, zpp=zpp):
    k = (zk - zpp) / (zk - z)
    return np.array([
        [k, 0, 0, 0],
        [0, k, 0, 0],
        [0, 0, 1, -zpp],
        [0, 0, 0, 1]
    ])

def create_formochka_points():
    points = []
    for k in np.linspace(-0.5, 0.5, M):
        for a in np.linspace(0, 2 * math.pi, N):
            x = R * (1 + abs(math.sin(2 * a))) * math.sin(a)
            y = R * (1 + abs(math.sin(2 * a))) * math.cos(a)
            z = H * k
            points.append(np.array([x, y, z, 1]))
    return points

def rotate_points(points, angle_x, angle_y, angle_z):
    rotation_x = np.array([
        [1, 0, 0, 0],
        [0, math.cos(angle_x), -math.sin(angle_x), 0],
        [0, math.sin(angle_x), math.cos(angle_x), 0],
        [0, 0, 0, 1]
    ])

    rotation_y = np.array([
        [math.cos(angle_y), 0, math.sin(angle_y), 0],
        [0, 1, 0, 0],
        [-math.sin(angle_y), 0, math.cos(angle_y), 0],
        [0, 0, 0, 1]
    ])

    rotation_z = np.array([
        [math.cos(angle_z), -math.sin(angle_z), 0, 0],
        [math.sin(angle_z), math.cos(angle_z), 0, 0],
        [0, 0, 1, 0],
        [0, 0, 0, 1]
    ])

    rotation_matrix = np.dot(rotation_z, np.dot(rotation_y, rotation_x))
    return [np.dot(rotation_matrix, point) for point in points]

if __name__ == "__main__":
    clock = pygame.time.Clock()
    points = create_formochka_points()

    angle_x, angle_y, angle_z = 0, 0, 0
    pos_x, pos_y = 0, 0
    scale = 1.0
    current_size = (INIT_WIDTH, INIT_HEIGHT)

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == QUIT:
                running = False
            elif event.type == VIDEORESIZE:
                current_size = (event.w, event.h)
                screen = pygame.display.set_mode(current_size, pygame.RESIZABLE)

        keys = pygame.key.get_pressed()

        # Rotate
        if keys[K_LEFT]:
            angle_y += 0.05
        if keys[K_RIGHT]:
            angle_y -= 0.05
        if keys[K_UP]:
            angle_x += 0.05
        if keys[K_DOWN]:
            angle_x -= 0.05
        if keys[K_q]:
            angle_z += 0.05
        if keys[K_e]:
            angle_z -= 0.05

        # Move
        if keys[K_w]:
            pos_y -= 5
        if keys[K_s]:
            pos_y += 5
        if keys[K_a]:
            pos_x -= 5
        if keys[K_d]:
            pos_x += 5

        # Scale
        if keys[K_j]:
            scale *= 1.02
        if keys[K_k]:
            scale /= 1.02

        screen.fill(BLACK)

        rotated_points = rotate_points(points, angle_x, angle_y, angle_z)

        projected_points = []
        for point in rotated_points:
            x, y, z, w = point
            proj_mat = projection_matrix(z)
            projected_point = np.dot(proj_mat, point)
            projected_point = projected_point / projected_point[3]
            projected_points.append(projected_point[:2])

        base_scale = min(current_size[0]/300, current_size[1]/250)
        center_x, center_y = current_size[0] // 2, current_size[1] // 2
        centered_points = []
        for point in projected_points:
            x = point[0] * scale * base_scale + center_x + pos_x
            y = point[1] * scale * base_scale + center_y + pos_y
            centered_points.append((x, y))

        point_size = max(1, int(scale * base_scale / 1.5))
        for point in centered_points:
            pygame.draw.circle(screen, WHITE, (int(point[0]), int(point[1])), point_size)

        line_width = max(1, int(scale * base_scale / 3))

        for k in range(M):
            for i in range(N - 1):
                idx1 = k * N + i
                idx2 = k * N + i + 1
                pygame.draw.line(screen, BLUE, centered_points[idx1], centered_points[idx2], line_width)
            pygame.draw.line(screen, BLUE, centered_points[k * N + N - 1], centered_points[k * N], line_width)

        for i in range(N):
            for k in range(M - 1):
                idx1 = k * N + i
                idx2 = (k + 1) * N + i
                pygame.draw.line(screen, RED, centered_points[idx1], centered_points[idx2], line_width)

        font = pygame.font.SysFont('Arial', 16)
        instructions = [
            "Управление:",
            "Стрелки ←→: вращение по Y",
            "Стрелки ↑↓: вращение по X",
            "Q/E: вращение по Z",
            "W/S: перемещение вверх/вниз",
            "A/D: перемещение влево/вправо",
            "J/K: масштабирование"
        ]

        for i, text in enumerate(instructions):
            text_surface = font.render(text, True, WHITE)
            screen.blit(text_surface, (10, 10 + i * 20))

        pygame.display.flip()
        clock.tick(60)

    pygame.quit()
