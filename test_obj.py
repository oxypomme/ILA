# Ceci est le résultat souhaité du ficher test.ila

nombre = 0 # Variables étant initialisées à 0 si non constantes
resultat = 0
pi = 3.141592
tab = {(0):0, (1):0, (2):0} # tableau devant un dict, histoire de pas avoir des listes de listes de listes...
i = 0
message = ""
bou = False


def fnc_carre(nombre):
    out = nombre * nombre
    return out

def modu_resultat(resultat):
    print(resultat)
    out0 = "yay"
    out1 = False
    return out0, out1

def Carre():
    input(nombre)
    bou = False
    print(tab[(1)])
    bou = True
    resultat = fnc_carre(nombre)
    message, bou = modu_resultat(resultat)
    if message == "yay":
        for i in range(0,5):
            print(message)
            
            
Carre()