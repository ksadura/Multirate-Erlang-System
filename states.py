C = 4 # number of servers in system

trunks = 2 # number of classes
sizes = [1, 2] # next class' sizes
lambdas = [0.75, 0.75] # next class' lambdas
mu = [1.0, 0.5]  # next class' mu

assert trunks == len(sizes), "Not enough elements in 'sizes' list"

from itertools import product
import math

def filters(x):
    t = [x[i] * sizes[i] for i in range(trunks)]
    return sum(t) <= C

perms = product(list(range(C + 1)), repeat = trunks)
states = list(filter(filters, perms))

def block_f(x):
    t = [x[i] * sizes[i] for i in range(trunks)]
    return sum(t) <= C - sizes[1] # change sizes[1] to sizes[0] to count class 1 blocking probability

perms = product(list(range(C + 1)), repeat = trunks)
blocks = list(filter(block_f, perms))

print(blocks)

t = 0
for state in states[1:]:
    q = 1
    for i in range(len(state)):
        q *= pow(lambdas[i]/mu[i], state[i]) / math.factorial(state[i])
    t += q

p0 = 1 / (1 + t)
print(f'P0 state probability: {p0}')

pb = 1

for state in states[1:]:
    q = 1
    r = 0
    for i in range(len(state)):
        q *= pow(lambdas[i]/mu[i], state[i]) / math.factorial(state[i])
    r = p0 * q
    print(f'State {state} probability: {r}')
    if state in blocks:
        pb -= r

print('Class ? block prob:', pb-p0)