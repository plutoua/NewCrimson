import numpy as np
import random


def generate_map(size, jump_spots):
    """
    Generates a map of given size.
    - 0 represents paths.
    - 1 represents walls.
    - 2 represents jump spots.
    """
    # Initialize the map with walls
    map = np.ones((size, size), dtype=int)
    map2 = np.ones((size, size), dtype=int)

    # Randomly create paths and jump spots
    for i in range(size):
        for j in range(size):
            if map[i, j] == 1:
                r = random.randint(0, 831)

                if r in range(0, 800):  # chance to be a path
                    map[i, j] = 0
                elif r in range(800, 810):  # chance to be a jump spot
                    map[i, j] = 2
                    jump_spots -= 1
                elif r in range(820, 830):  # chance to be an enemy
                    map[i, j] = 4
                elif r in range(830, 831):  # chance to be a building
                    map[i, j] = 5
                    map[i - 1, j] = 5
                    map[i - 1, j + 1] = 5
                    map[i, j + 1] = 5



    shores = [[],[],[],[]]
    for shore in shores:
        for i in range(size):
            shore_limits = (2, 11)
            random_number = random.randint(shore_limits[0], shore_limits[1])
            shore.append(random_number)

    shore_i = 0
    for shore in shores:
        del_1 = 1
        del_2 = 2
        # shore [1,2,5,12,61,2,3]
        line_j = 0
        for line in shore:
            # line 1
            num_to_pass = 0
            while num_to_pass < line:
                temp_1 = line_j
                temp_2 = num_to_pass
                if shore_i == 1:
                    temp_2 = map_size - temp_2 - 1

                if shore_i == 2:
                    t = temp_2
                    temp_2 = temp_1
                    temp_1 = t

                if shore_i == 3:
                    t = temp_2
                    temp_2 = temp_1
                    temp_1 = -t

                map[temp_1, temp_2] = 1
                num_to_pass += 1
            line_j += 1
        shore_i += 1

    return map

# Generate a 100x100 map with a limited number of jump spots
map_size = 100
jump_spots = 200  # You can adjust the number of jump spots
generated_map = generate_map(map_size, jump_spots)
with open("map1.map", 'w') as f:
    for row in generated_map:
        f.write(" ".join(map(str, row)) + "\n")

    f.close()
print(generated_map)
