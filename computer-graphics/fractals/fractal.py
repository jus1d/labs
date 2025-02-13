LINE_LENGTH = 20
ALPHA = 60
AXIOM = 'FXF--FF--FF'
RULES = {
    'F': 'FF',
    'X': '--FXF++FXF++FXF--',
}

def generate_next(s: str) -> str:
    next = ''
    for ch in s:
        if ch in RULES:
            next += RULES[ch]
        else:
            next += ch
    return next

def generate(n: int) -> str:
    s = AXIOM
    for i in range(n):
        s = generate_next(s)
    return s
