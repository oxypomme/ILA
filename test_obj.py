# Ceci est un commentaire (slt du coup)
# Ceci est le résultat souhaité du ficher test.ila

# ici on déclare les variables de l'algo
nombre = 0 # Nombre à traiter
resultat = 0 # Carré calculé
pi = 3.141592 #une constante
t_tab = {(0):0, (1):0, (2):0} #tableau d'entiers
t_multiTab = {(0, 0): '', (0, 1): '', (0, 2): '', (0, 3): '', (0, 4): '', (1, 0): '', (1, 1): '', (1, 2): '', (1, 3): '', (1, 4): '', (2, 0): '', (2, 1): '', (2, 2): '', (2, 3): '', (2, 4): '', (3, 0): '', (3, 1): '', (3, 2): '', (3, 3): '', (3, 4): '', (4, 0): '', (4, 1): '', (4, 2): '', (4, 3): '', (4, 4): '', (5, 0): '', (5, 1): '', (5, 2): '', (5, 3): '', (5, 4): '', (6, 0): '', (6, 1): '', (6, 2): '', (6, 3): '', (6, 4): '', (7, 0): '', (7, 1): '', (7, 2): '', (7, 3): '', (7, 4): '', (8, 0): '', (8, 1): '', (8, 2): '', (8, 3): '', (8, 4): '', (9, 0): '', (9, 1): '', (9, 2): '', (9, 3): '', (9, 4): ''}
class t_struct:
    variable1 = 0
    variable2 = ""

t_joursSemaine = ["LUNDI", "MARDI", "MERCREDI", "JEUDI", "VENDREDI", "SAMEDI", "DIMANCHE"]
tab = t_tab
variableStruct = t_struct()
i = 0
bou = False
message = ""
aujourdhui = t_joursSemaine

"""
    Une fonction qui calcule le carré d'un nombre.
    le carré, c'est quand on le multiplie par lui-même 
"""
def fnc_carre(nombre):
    return nombre * nombre

#le mode par défaut est ENTRÉE, donc le "e::" est optionnel
def modu_resultat(resultat):
    print(resultat)
    out0 = "yay"
    out1 = False
    while True:
        out1 = True
        if not False:
            break
    return out0, out1

# Un seul algo par fichier ! > Pour differencier l'algo principal d'un module ou autre
def Carre():
    ajourdhui = t_joursSemaine[1] # le type est automatiquement reconnu s'il n'existe pas en doublon
    nombre = int(input())
    bou = False
    print(tab[(1)])
    bou = True
    variableStruct.variable1 = 5
    variableStruct.variable2 = "une belle structure"
    resultat = fnc_carre(nombre)
    message, bou = modu_resultat(resultat)
    if message == "yay":
        for i in range(0,5):
            print(message)
    while i<10:
        print(i)
        i = i + 1
    #ftq

#opérateurs : + - * / mod div et ou non <- < <= > >= = !=

Carre()
