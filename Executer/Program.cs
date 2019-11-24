using System;
using ILANET;
using ILANET.Parser;

namespace Executer
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            var prg = Parser.Parse(
@"
// Ceci est un commentaire (slt du coup) Ce fichier est là pour tester et donne un exemple fonctionnel

//ici on déclare les variables de l'algo
nombre:entier // Nombre à traiter
resultat:entier // Carré calculé
pi:const reel <- 3.141592 //une constante
t_tab:type tableau[1..3] : entier //tableau d'entiers, on peut aus
t_multiTab: type tableau[1..10, 1..5]: chaine //tableau à deux dimensions
 t_struct:type structure(
     variable1:entier,
    variable2: chaine
)   //on peut aussi écrire
t_joursSemaine: type enumeration(
         LUNDI,
         MARDI,
         MERCREDI,
         JEUDI,
         VENDREDI,
         SAMEDI,
         DIMANCHE

     ) //on peut aussi écrir
tab: t_tab
 variableStruct:t_struct
 i:entier
 bou:booleen
 message:chaine
 aujourdhui:t_joursSemaine

 //Un seul algo par fichier ! > Pour differencier l'algo principal d'un module ou autre
 algo Carre
 {
    aujourdhui <- MARDI //le type est automatiquement reconnu s'il n'existe pas en doublon
    lire(nombre)
     bou <- faux
     ecrire(tab[2])
     bou <- vrai
     variableStruct.variable1 <- 5
     variableStruct.variable2 <-""une belle structure""
     resultat <-fnc_carre(nombre)
     modu_resultat(resultat, message, bou)
     si message = ""yay"" alors
        pour i <-1 a 5 faire
             ecrire(message)
         fpour
        pour i <-5 a 1 pas -1 faire
             ecrire(message)
         fpour
    fsi
     /*
        sinon si vrai alors
        sinon
    */
    tantque i<10 faire
         ecrire(i)

        i <- i + 1
     ftantque //ftq
}
/*
    Une fonction qui calcule le carré d'un nombre.
    le carré, c'est quand on le multiplie par lui-même
*/
fonction fnc_carre(nombre:entier):entier
{
     fnc_carre <-nombre * nombre
}
//le mode par défaut est ENTRÉE,
module modu_resultat(e::resultat:entier, es::message:chaine, s::bou:booleen)
variable_locale: chaine
{
    ecrire(resultat)
    message <-variable_locale
    bou <-faux
    repeter
        bou <-vrai
    jusqua faux
}

//opérateurs : + - * / mod div et ou non <- < <= > >= = !=
");
            Console.WriteLine(prg.Declarations.Count);
        }

        #endregion Private Methods
    }
}