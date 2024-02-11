import random
import numpy as np
import math

def generate_simple_maze(x, y, exits):
    # Initialize the maze grid, 1 represents walls, 0 represents paths
    maze = np.ones((y, x), dtype=np.int8)

    # Function to carve paths from the current cell to the next
    def carve_path(x, y):
        directions = [(0, -1), (1, 0), (0, 1), (-1, 0)]  # Up, Right, Down, Left
        random.shuffle(directions)  # Randomize directions

        for dx, dy in directions:
            nx, ny = x + dx * 2, y + dy * 2  # Move to the next cell in chosen direction

            if 0 <= nx < len(maze[0]) and 0 <= ny < len(maze):  # Check bounds
                if maze[ny][nx] == 1:  # If the next cell is a wall
                    maze[ny - dy][nx - dx] = 0  # Remove wall between cells
                    maze[ny][nx] = 0  # Carve path in the next cell
                    carve_path(nx, ny)  # Recursively carve paths from the next cell

    # Start carving from the center (or near center if even dimensions)
    start_x, start_y = x // 2, y // 2
    if start_x % 2 == 0: start_x -= 1
    if start_y % 2 == 0: start_y -= 1
    maze[start_y][start_x] = 0

    # Begin carving paths from the start position
    carve_path(start_x, start_y)

    # Add exits
    edge_cells = []
    for i in range(y):
        if i == 0 or i == y - 1:  # Top and bottom edges
            for j in range(1, x - 1, 2):
                edge_cells.append((j, i))
        else:  # Side edges
            edge_cells.append((0, i))
            edge_cells.append((x - 1, i))
    random.shuffle(edge_cells)

    for _ in range(exits):
        if edge_cells:
            ex, ey = edge_cells.pop()
            maze[ey][ex] = 0

    return maze.tolist()

def enlarge_maze(maze, scale=5):
    enlarged_maze = np.ones((maze.shape[0]*scale, maze.shape[1]*scale), dtype=np.int8)
    for y in range(maze.shape[0]):
        for x in range(maze.shape[1]):
            if maze[y, x] == 0:  # Якщо це прохід
                for dy in range(scale):
                    for dx in range(scale):
                        enlarged_maze[y*scale + dy, x*scale + dx] = 0  # Робимо великий прохід
    return enlarged_maze

def randomize_paths(maze, scale=5, min_width=1, max_width=5):
    for y in range(0, maze.shape[0], scale):
        for x in range(0, maze.shape[1], scale):
            if maze[y, x] == 0:  # Якщо це прохід
                path_width = random.randint(min_width, max_width)
                # Розширюємо прохід
                for dy in range(min(scale, path_width)):
                    for dx in range(min(scale, path_width)):
                        if y+dy < maze.shape[0] and x+dx < maze.shape[1]:
                            maze[y+dy, x+dx] = 0
    return maze


# cave_cariant
def generate_maze_cave_sequential(x, y, start_points):
    maze = np.ones((y, x), dtype=np.int8)  # Initialize the maze with walls

    # Directions: Up, Right, Down, Left
    directions = [(0, -1), (1, 0), (0, 1), (-1, 0)]

    def valid_move(nx, ny):
        """Check if the move is within bounds and not creating isolated sections."""
        return 0 <= nx < x and 0 <= ny < y and maze[ny][nx] == 1

    def carve_from(x, y):
        """Attempt to carve a path from the given position."""
        possible_directions = directions[:]
        random.shuffle(possible_directions)  # Randomize directions

        for dx, dy in possible_directions:
            nx, ny = x + dx, y + dy

            if valid_move(nx, ny):
                # Carve path
                maze[ny][nx] = 0
                # Recursively carve paths from the new cell
                carve_from(nx, ny)
                break  # Only carve one direction per call in this approach

    # Carve paths sequentially from each start point
    for sp in start_points:
        sx, sy = sp
        maze[sy][sx] = 0  # Mark the start point as part of the path
        carve_from(sx, sy)

    return maze.tolist()

class AdvBase:

    def __init__(self, adv_type=0, name="base", all_size=None):
        if all_size is None:
            all_size = [30, 30]
        self.adv_type = adv_type
        self.name = name
        self.all_size = all_size

class AdvMaze(AdvBase):
    def __init__(self, all_size, exits=3):
        super().__init__(1, "maze", all_size)
        self.exits = exits

class AdvCaveMaze(AdvBase):
    def __init__(self, all_size=[21,21], start_points=[(1, 1), (19, 1), (1, 19), (19, 19)]):
        super().__init__(6, "maze_cave", all_size)
        self.start_points = start_points

class AdvCrossroad(AdvBase):
    def __init__(self, all_size=None):
        super().__init__(2, "crossroad", all_size)

class AdvCamp(AdvBase):
    def __init__(self, all_size=None):
        super().__init__(3, "camp", all_size)

class AdvTown(AdvBase):
    def __init__(self, all_size=None):
        super().__init__(4, "town", all_size)



class ObjsParent:
    def __init__(self, new_chank=False):
        # тип генерованого обєкту для карти
        self.type = 0
        # для зєднаних обєктів (стіни, дороги)
        self.draw_cells = []
        # кут повороту, тільки для простих обєктів
        self.angle = 0
        # координати, в яких зберігається обєкт
        self.coords = []
        # трансферна властивість для передачі координат
        self.strict_coords = []
        # ціль зєднаного обєкту (тип іншого або того ж обєкту. контроль послідовною подачею. для звязаних доріг)
        self.destination_type = 0
        # буль значення для переривання малювання чанку. починаєм з нової точки (цієї)
        self.new_chank = new_chank
        # малювання внутрянки обєкту і його розмітки іншими обєктами по потребі. формат - {тип: [[координати],[координати]]}

        self.additional_cells = {}

        # 17 - road, 1 - wall


long_types = [17, 1]
class Wall(ObjsParent):
    def __init__(self):
        super().__init__()
        self.type = 1
        self.draw_cells = [[0,0], [0,1]]
        self.angle = 1
        self.destination_type = 17

class Table(ObjsParent):
    def __init__(self):
        super().__init__()
        self.type = 2
        self.draw_cells = [[0,0], [1,0], [0,1], [1,1]]

class PigTavern(ObjsParent):
    def __init__(self):
        super().__init__()
        self.type = 5
        self.draw_cells = [[0,0], [1,0], [0,1], [1,1]]
        # self.additional_cells = {3: [[0,1]]}

class BigPigTavern(ObjsParent):
    def __init__(self):
        super().__init__()
        self.type = 13
        self.draw_cells = [[0,0],[0,1],[0,3],[0,4],[1,0],[1,4],[2,0],[2,4],[3,0],[3,4],[4,0],[4,1],[4,3],[4,4],[0,2],[4,2]]
        self.additional_cells = {17: [[0,2], [4,2]]}




class WorkTable(ObjsParent):
    def __init__(self):
        super().__init__()
        self.type = 3
        self.draw_cells = [[0,0], [1,0], [2,0]]

def rotate_cells(cells, angle):
    if angle == 90:
        return [[-y, x] for x, y in cells]
    elif angle == 180:
        return [[-x, -y] for x, y in cells]
    elif angle == 270:
        return [[y, -x] for x, y in cells]
    else:
        return cells



class Road(ObjsParent):
    def __init__(self, new_chank=False):
        super().__init__(new_chank)
        self.type = 17
        self.destination_type = 17
        self.draw_cells = [[0,0]]  # Will be calculated during road generation
        self.coords = []

    def set_coords(self, coords):
        self.draw_cells = coords

# class Destination:
#     def __init__(self):
#         self.type = 18
#         # self.destination_type = 18
#         self.draw_cells = [[0,0],[5,5]]  # Will be calculated during road generation

def can_place(square, obj, x, y, no_adjacent_types):
    #print(obj)
    for cell in obj.draw_cells:
        dx, dy = cell
        new_x, new_y = x + dx, y + dy
        if new_x >= len(square) or new_y >= len(square[0]) or square[new_x][new_y] != 0:
            return False
        # Check adjacent cells for restricted types
        for adj_x, adj_y in [(new_x-1, new_y), (new_x+1, new_y), (new_x, new_y-1), (new_x, new_y+1)]:
            if 0 <= adj_x < len(square) and 0 <= adj_y < len(square[0]):
                if square[adj_x][adj_y] in no_adjacent_types:
                    return False
    return True

import heapq

def heuristic(a, b):
    """Оцінює відстань між двома точками."""
    (x1, y1) = a
    (x2, y2) = b
    return abs(x1 - x2) + abs(y1 - y2)

def a_star_search(grid, start, goal, type):
    """Виконує пошук A* на сітці."""
    neighbors = [(0, 1), (1, 0), (0, -1), (-1, 0)]  # сусідні напрямки (4-связность)

    close_set = set()
    came_from = {}
    gscore = {start: 0}
    fscore = {start: heuristic(start, goal)}
    open_set = []

    heapq.heappush(open_set, (fscore[start], start))

    while open_set:
        current = heapq.heappop(open_set)[1]

        if current == goal:
            path = []
            while current in came_from:
                path.append(current)
                current = came_from[current]
            return path[::-1]

        close_set.add(current)
        for i, j in neighbors:
            neighbor = current[0] + i, current[1] + j
            tentative_g_score = gscore[current] + heuristic(current, neighbor)
            if 0 <= neighbor[0] < len(grid):
                if 0 <= neighbor[1] < len(grid[0]):
                    if grid[neighbor[0]][neighbor[1]] not in (0, type):  # перевірка на пустоту і самого себе
                        continue
                else:
                    # Якщо ми вийшли за межі сітки по осі Y
                    continue
            else:
                # Якщо ми вийшли за межі сітки по осі X
                continue

            if neighbor in close_set and tentative_g_score >= gscore.get(neighbor, 0):
                continue

            if tentative_g_score < gscore.get(neighbor, 0) or neighbor not in [i[1] for i in open_set]:
                came_from[neighbor] = current
                gscore[neighbor] = tentative_g_score
                fscore[neighbor] = tentative_g_score + heuristic(neighbor, goal)
                heapq.heappush(open_set, (fscore[neighbor], neighbor))

    return False

def is_valid(square, x, y):
    return 0 <= x < len(square) and 0 <= y < len(square[0]) and square[x][y] == 0

def generate_road(square, from_obj, to_obj, algorithm=None):
    x1, y1 = from_obj.coords[0]
    x2, y2 = to_obj.coords[0]

    if algorithm == "astar":
        path = a_star_search(square, (x1, y1), (x2, y2), from_obj.type)
        if path:
            for x, y in path:
                square[x][y] = from_obj.type  # A* path type
    else:
        # Ваша існуюча логіка
        dx = 1 if x2 > x1 else -1 if x2 < x1 else 0
        dy = 1 if y2 > y1 else -1 if y2 < y1 else 0
        x, y = x1, y1

        while (x, y) != (x2, y2):
            if x != x2 and is_valid(square, x + dx, y):
                x += dx
            elif y != y2 and is_valid(square, x, y + dy):
                y += dy
            else:
                break

            if 0 <= x < len(square) and 0 <= y < len(square[0]):
                square[x][y] = from_obj.type  # Basic path type

def place_objects_in_square(size, objects, no_adjacent_types, preprepared_square=None):
    #print(preprepared_square)
    if preprepared_square:
        square = preprepared_square
    else:
        square = [[0 for _ in range(size[1])] for _ in range(size[0])]
    #print(square)

    for obj in objects:
        placed = False
        while not placed:
            x = random.randint(0, size[0] - 1)
            y = random.randint(0, size[1] - 1)
            cycles = 10
            if cycles > 0 and can_place(square, obj, x, y, no_adjacent_types):

                for cell in obj.draw_cells:
                        # if obj.type == 5:
                        #     print("+")

                        dx, dy = cell
                        temp_dx = dx
                        temp_dy = dy
                        # if obj.strict_coords:
                        #     obj.coords.append(obj.replasment_coords)
                        #     temp_dx = obj.replasment_coords.x
                        #     temp_dy =obj.replasment_coords.y

                        temp_type = obj.type
                        for k, v in obj.additional_cells.items():
                            if cell in v:
                                temp_type = k



                        square[x + temp_dx][y + temp_dy] = temp_type


                        obj.coords.append([x, y])
                        placed = True

                cycles += 1

    return square



def draw_shit(size, draw_things, preprepared_square=None):

    no_adjacent_types = [Wall().type]  # Example of types that can't be adjacent

    square_filled = place_objects_in_square(size, draw_things, no_adjacent_types, preprepared_square=preprepared_square)

    #for row in square_filled:

        # print(row)

    # допоміжний лічильник для того, щоб малювати дороги (стіни) парами. відповідно має бути і початок і кінець відрізку.
    line_index = 1



    # Adding lines between objects of type 17 and 1
    for i in range(len(draw_things)):
        if draw_things[i].type in long_types:

            for j in range(i + 1, len(draw_things)):
                if draw_things[j].type == 1:

                    line_index += 1
                    if (line_index % 2 == 0):
                        generate_road(square_filled, draw_things[i], draw_things[j])
                    break

                if draw_things[j].type == 17:

                    line_index += 1
                    if (line_index % 2 == 0):
                        generate_road(square_filled, draw_things[i], draw_things[j], algorithm="astar")
                    break
    return square_filled

# Example usage

basic = 0
labyrynth = 1

# base, maze, crossroad, camp, town
types_for_generation = 5
x = random.randint(0, types_for_generation)

what_to_draw = {
    AdvBase(all_size=[40, 50]): [Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(),
                    Wall(), Wall(), Wall(), Wall(), Wall(), Road(), Road(), Road(), Road(), Road(), Road(),
                    PigTavern()],

    AdvBase(all_size=[45, 30]):            [Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(),
                    Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Wall(), Road(), Road(), Road(), Road(), Road(), Road(), Road(), Road(), Road(), Road(), Road(), Road()],

    AdvMaze(all_size=[45, 30]): [],

    AdvCamp(all_size=[10, 10]): [Road(), Road(), Road(), Road(), Road(), Road(), BigPigTavern()],

    AdvTown(all_size=[10, 10]): [Road(), Road(), Road(), Road(), Road(), Road(), BigPigTavern()],


    # AdvCrossroad(): []
}
shits = []

for k,v in what_to_draw.items():

    preprepared_square = None
    if k.name == "maze_cave":

        preprepared_square = generate_maze_cave_sequential(k.all_size[0], k.all_size[1], k.start_points)
    if k.name == "maze":
        preprepared_square = generate_simple_maze(k.all_size[0], k.all_size[1], k.exits)

    shits.append(draw_shit(k.all_size, v, preprepared_square))

# arrays = [first_shit, second_shit, third_shit, four_shit, five_shit, six_shit]

def combine_arrays(arrays, array_size):
    # Визначаємо кількість масивів, що будуть розміщені по кожній стороні виходного масиву
    arrays_per_side = math.ceil(math.sqrt(len(arrays)))

    # Розрахунок розміру сторони виходного масиву
    side_length = arrays_per_side * array_size
    output = [[0 for _ in range(side_length)] for _ in range(side_length)]

    # Розміщення кожного масиву в output
    for arr_index, arr in enumerate(arrays):
        # Визначення стартової позиції для кожного масиву
        start_x = (arr_index % arrays_per_side) * array_size
        start_y = (arr_index // arrays_per_side) * array_size

        for i, row in enumerate(arr):
            for j, value in enumerate(row):
                output[start_y + i][start_x + j] = value

    return output

padding = 2
# Розрахунок оптимального розміру секції
max_arr_width = max(len(arr[0]) for arr in shits) + padding
max_arr_height = max(len(arr) for arr in shits) + padding
optimal_section_size = max(max_arr_width, max_arr_height)

# # Визначення оптимального розміру вихідного масиву
# optimal_side_length = optimal_section_size * 2
# print(optimal_side_length)


# Об'єднання масивів
output_map = combine_arrays(shits, optimal_section_size)


for row in output_map:
    print(" ".join(str(cell) for cell in row))

#
# # Об'єднання масивів
# output_map = combine_arrays_with_padding(arrays)
# for row in output_map:
#     print(row)