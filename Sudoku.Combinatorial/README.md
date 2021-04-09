# Projet C# Sudoku Solver

Sarah Vasquez, Laureline Petit, Vada Zamble, Marie-Jo Kone

## Sujet 11 : Résolution de Sudoku par réseau de neurones convolués

### Collecte des données

Nous avons trouvé les données sur Kaggle qui contiennent 1 million de jeux Sudoku non résolus et résolus sur deux colonnes. Chaque jeu représente une chaîne de 81  numéros.  Le nombre 0 représente la position vide dans les jeux non résolus.

![screen1](screens/screen1.png)

Notre tâche est de nourrir le sudoku non résolu grâce à un réseau neuronal pour obtenir le sudoku résolu en sortie. En d'autres termes, cela veut dire qu'en entrée on alimente le réseau de 81 numéros et en sortie, on obtient 81 numéros. Les chiffres de 1 à 9 prenant la place des 0.
 
Pour cela, nous devons convertir les données d'entrées non résolues en un tableau 3D puis les normaliser car les réseaux de neurones fonctionnent généralement mieux avec des données normalisées centrées.
 
### Conception de réseau

Dans une classification multiclasse typique, le réseau neuronal produit des scores pour chaque classe. Ensuite, nous appliquons la fonction softmax sur les scores finaux pour les convertir en probabilités. Et les données sont classées dans une classe qui a la valeur de probabilité la plus élevée.

![screen2](screens/screen2.png)

Mais dans le sudoku, le scénario est différent. Nous devons obtenir 81 numéros pour chaque position dans le jeu sudoku, pas seulement un. Et nous avons un total de 9 classes pour chaque nombre parce qu’un nombre peut tomber dans une gamme de 1 à 9.
Pour se conformer à cette conception, notre réseau doit avoir  les numéros de sortie où chaque ligne représente l’un des 81 numéros, et chaque colonne représente l’une des 9 classes. Ensuite, nous pouvons appliquer softmax et prendre le maximum avec chaque ligne afin que nous ayons 81 numéros classés dans l’une des 9 classes => 81 x 9
Suivant cette tâche, le réseau se compose de 3 couches de convolution et d’une couche dense sur le dessus pour la classification.

La sortie de la couche dense a été transformée en 81 x 9 en ajoutant une couche Softmax.
Pour compiler le modèle, La fonction loss utilisée  est la fonction "sparse_categorical_crossentropyadam" et l'optimizer est "Adam".
 
Le modèle a été entraîné avec 2 epochs et un batch-size de 64. Le taux d'apprentissage pour le premier "epoch" est de 0.001 et de 0.0001 pour le deuxième.

### Résolution du Sudoku 

Lors de la résolution du Sudoku , le réseau prédit peu de valeurs erronées. Une approche humaine pour résoudre le jeu est de remplir les numéros un par un.
Nous ne regardons pas simplement le sudoku une fois pour remplir tous les nombres. L’avantage de remplir les numéros un par un, c’est que chaque fois que nous remplissons un numéro, nous continuons à avoir une meilleure idée de la prochaine étape.

Cette approche a été mise en œuvre tout en résolvant le sudoku. Au lieu de prédire tous les 81 numéros à la fois,un numéro est choisi parmi toutes les positions vierges qui a la valeur de probabilité la plus élevée. Après avoir rempli un numéro, nous alimentons à nouveau ce puzzle au réseau et faisons une prédiction. Nous répétons cela et remplissons les positions vierges une par une avec le nombre de probabilité le plus élevé jusqu’à ce que nous soyons sans positions vides.

Cette approche a stimulé les performances et le réseau a été en mesure de résoudre presque tous les jeux dans cet ensemble de données. La précision des tests sur 1000 jeux était de 0,99.
 
Les librairies suivantes ont été utilisées pour porter le code en c#: 
* Keras.Net
* Tensorflow.Net

```c#

// Modele CNN
var model = new Sequential();

model.Add(new Conv2D(64, kernel_size: (3, 3).ToTuple(), padding: 'same', activation: "relu", input_shape: (9, 9, 1));
model.Add(new BatchNormalization());
model.Add(new Conv2D(64, (3, 3).ToTuple(), padding: 'same', activation: "relu"));
model.Add(new BatchNormalization());
model.Add(new Conv2D(128, (1, 1).ToTuple(), padding: 'same', activation: "relu"));


model.Add(new Flatten());
model.Add(new Dense(81*9));
model.Add(new Reshape((-1, 9)));
model.Add(new Activation('softmax'));

```

Nous avons malheureusement pas réussi à faire tourner sur nos machines le code en entier car des erreurs liées à Python et Tensorflow ont persisté ne nous permettant pas de finaliser la résolution du sudoku par réseau de neurones convolués. Nous avons donc décidé de tester le sujet 4 qui n'avait pas été choisi par un groupe afin de pouvoir essayer de terminer un projet jusqu'au bout.

## Sujet 4 : Résolution de Sudoku à l’aide de l’évolution combinatoire

### Introduction

Ces dernières années, des progrès considérables ont été réalisés dans les domaines des neurosciences, des sciences cognitives et de la physiologie en ce qui concerne la manière dont les êtres humains traitent l'information. La recherche actuelle dans l'informatique est impactée par ces progrès notamment dans les domaines de neurosciences computationnelles, de sciences cognitives, de biologie et de méthodes computationnelles inspirées de l'évolution. Il existe tout un tas d’algorithmes directement inspirés par la nature. 

Par exemple, il y a celui de l’évolution différentielle (ED). Très similaire aux recherches de motifs et aux algorithmes génétiques, il utilise le croisement et la mutation. L’ED peut être considéré comme un développement des algorithmes génétiques à quelques différences près. Il est un algorithme de recherche stochastique à tendance auto-organisatrice, basé sur la population sans dérivés en utilisant des nombres réels comme chaînes de solutions, ce qui ne nécessite ni encodage ni décodage. Il effectue des opérations sur chaque composant (ou dimension de la solution) et tout est fait en termes de vecteurs. Cette mutation vectorielle peut être considérée comme une approche plus efficace du point de vue de la mise en œuvre. Également, une perturbation est effectuée sur chaque vecteur de population et on peut donc s'attendre à ce qu'il soit plus efficace. 

Un autre exemple d’algorithme inspiré de la nature est celui de l’optimisation par essaims de particules (OEP ou PSO en anglais). Ici, on n’utilise pas le croisement ou la mutation, mais on se concentre sur le caractère aléatoire des nombres réels et la communication globale entre les particules de l’essaim. Il est également plus facile à mettre en œuvre, car il n'y a pas d’encodage ou de décodage. L'algorithme recherche l'espace d'une fonction « objectif » en ajustant les trajectoires des particules dans des vecteurs de position d'une manière quasi-stochastique. Le mouvement d'une particule en essaim est constitué de deux composantes principales : une composante stochastique et une composante déterministe. 

Un autre exemple d’algorithme inspiré de la nature est celui du comportement de la luciole (Algorithmes Firefly, AF). Cet algorithme est directement inspiré de la lumière clignotante des lucioles qui a pour fonction d’attirer des partenaires d’accouplement (communication) ou d’attirer des proies potentielles. Son intensité lumineuse diminue lorsque la distance augmente. Le clignotement de la lumière peut être formulé de manière à être associé à la fonction « objectif » à optimiser, ce qui permet de formuler de nouveaux algorithmes d'optimisation. L'attrait est proportionnel à la luminosité, les deux diminuant à mesure que leur distance augmente. S'il n'y a pas de luciole plus brillante qu'une autre, le déplacement se fait de façon aléatoire.

Cependant les performances réalisées par ces algorithmes peuvent être contrastées, comme il a été fait par Adam P. Piotrowski, Maciej J. Napiorkowski, Jaroslaw J. Napiorkowski et Pawel M. Rowinski dans leur article « Swarm Intelligence and Evolutionary Algorithms: Performance versus speed ». Dans leur papier ils ont testé et démontré que les algorithmes d'optimisation par essaims de particules sont plus performants lorsque le nombre d’appels de fonction autorisés est faible alors que l’évolution différentielle elle se montre plus performante lorsque ce nombre est élevé. Il est difficile de trouver un algorithme dont les performances seraient adéquates pour tous les nombres d'appels de fonctions testés. Ils ont aussi constaté que certains algorithmes peuvent devenir complètement non fiables sur des problèmes spécifiques du monde réel, même s'ils fonctionnent raisonnablement sur d'autres.

De ces algorithmes inspirés par la nature, découle notre algorithme de l’évolution combinatoire, qui a pour but d’organisé un ensemble d'éléments discrets dans un ordre particulier tout en étant confronté à un problème d’optimisation combinatoire qui peut par exemple être un sudoku comme dans notre projet.

Un problème d'optimisation combinatoire consiste à trouver la meilleure solution dans un ensemble discret dit ensemble des solutions réalisables, il est décrit par une liste de contraintes que doivent satisfaire les solutions réalisables. Pour définir la notion de meilleure solution, une fonction « objectif » est introduite. Pour chaque solution, elle renvoie un réel et la meilleure solution (ou solution optimale) est celle qui minimise ou maximise la fonction « objectif ».

Par exemple, dans un sudoku, il y a plusieurs contraintes. Chaque ligne de la grille de solution 9x9 doit contenir les nombres de 1 à 9, sans doublons. Chaque colonne doit contenir les nombres 1 à 9. Chaque sous-grille 3x3 doit contenir les nombres 1 à 9. Et la grille de solution ne peut changer aucune des valeurs de départ dans la grille de problème.

L'optimisation de l'évolution combinatoire utilise des idées issues de plusieurs algorithmes bio-inspirés. L'algorithme maintient une collection d'organismes virtuels. Chaque organisme représente une solution possible au problème. L'évolution combinatoire est un processus itératif où à chaque itération, chaque organisme tente de trouver une meilleure solution en examinant une nouvelle solution possible.

Une fois que tous les organismes ont eu une chance de s'améliorer, deux bonnes solutions d'organismes sont sélectionnées et utilisées pour donner naissance à un nouvel organisme, qui remplace une mauvaise solution. Ainsi, la population d'organismes évolue avec le temps. Si une solution optimale n'est pas trouvée après un certain temps (nombre max d’itérations), l'algorithme est redémarré en tuant tous les organismes et en créant une nouvelle population. Ce processus de redémarrage a pour but de contrer la tendance de l’algorithme à se bloquer trop rapidement lorsqu’il trouve une très bonne solution. C’est une technique courante de nombreux algorithmes d’optimisation.

### Explication du programme 

Sur Visual Studio, nous avons créé le projet SudokuCombinatorialEvolutionSolver et ce programme n'a pas de dépendances.NET significatives.
 
La classe principale a toute la logique du code implémenté en tant que méthodes statiques tandis que la class Organism définit une solution possible au problème de Sudoku cible.
 
Chaque objet Organisme a un type, qui peut être 0 pour un organisme «Worker» ou 1 pour un organisme «Explorer». Le champ nommé "matrix" est une matrice de type tableau de tableaux d'entiers qui représente une solution possible. Chaque solution possible a une erreur, où une valeur d'erreur de 0 signifiant qu'aucune contrainte n'est violée et, par conséquent, le champ de matrice contient une solution optimale. Chaque objet Organism a un champ pour contrôler si l'organisme est tué à chaque epoch.

Le programme configure et affiche le problème Sudoku en utilisant ces instructions:

```c#

Console.WriteLine("Begin solving Sudoku using combinatorial evolution");
Console.WriteLine("The Sudoku is:");

var sudoku = Sudoku.Easy;
Console.WriteLine(sudoku.ToString());

```

La méthode ToString permet d'afficher l'état d'un Sudoku.

Notons que les valeurs 0 sont utilisées pour indiquer une cellule vide. Cette approche manuelle codée en dur est assez fastidieuse et dans la plupart des problèmes d'optimisation combinatoire réalistes, les données sont lues à partir d'un fichier texte.
 
Le problème du Sudoku est abordé à l'aide de ces déclarations :

```c#

const int numOrganisms = 200;
const int maxEpochs = 5000;
const int maxRestarts = 40;
   
var solver = new SudokuSolver();
var solvedSudoku = solver.Solve(sudoku, numOrganisms, maxEpochs, maxRestarts);


```

La méthode Solve est principalement un wrapper autour de la méthode SudokuSolver, qui effectue la majeure partie du travail. Il s'agit d'un modèle de conception courant dans l'optimisation combinatoire: une méthode de solveur de bas niveau tente de trouver une solution optimale, et cette méthode est encapsulée par une méthode de solveur de haut niveau qui effectue des redémarrages.

L'algorithme d'évolution combinatoire n'est pas garanti pour trouver une solution optimale (c'est-à-dire une solution sans erreur de contrainte), donc le programme de démonstration vérifie si la meilleure solution trouvée est optimale.


#### Initialisation et erreur de la matrice

Ici il s’agit de trouver la bonne méthode pour parcourir chaque sous grille. Dans ce jeu on a des contraintes de lignes, de colonnes et de blocs. Il faut remplir toutes les cases avec les chiffres de 1 à 9, mais pas n'importe comment. Dans chaque colonne, dans chaque ligne et dans chaque bloc il doit y avoir tous les chiffres de 1 à 9 tout en évitant qu’il y ait des doublons. La contrainte la plus efficace à utiliser pour éviter les doublons est celle de sous-grille. L’approche adoptée consiste à définir deux méthodes d'aide, Block et Corner.                                                                                                                   Method Block accepte un index de ligne r et un index de colonne c, et renvoie un numéro de bloc (0-8) qui contient la cellule à (r, c). Les numéros de bloc sont attribués de gauche à droite, puis de haut en bas.

```c#

public static int Block(int r, int c)
        {
            if (r >= 0 && r <= 2 && c >= 0 && c <= 2)
                return 0;
            if (r >= 0 && r <= 2 && c >= 3 && c <= 5)
                return 1;
            if (r >= 0 && r <= 2 && c >= 6 && c <= 8)
                return 2;
            if (r >= 3 && r <= 5 && c >= 0 && c <= 2)
                return 3;
            if (r >= 3 && r <= 5 && c >= 3 && c <= 5)
                return 4;
            if (r >= 3 && r <= 5 && c >= 6 && c <= 8)
                return 5;
            if (r >= 6 && r <= 8 && c >= 0 && c <= 2)
                return 6;
            if (r >= 6 && r <= 8 && c >= 3 && c <= 5)
                return 7;
            if (r >= 6 && r <= 8 && c >= 6 && c <= 8)
                return 8;

            throw new Exception("Unable to find Block()");
        }

```

Method Corner accepte un ID de bloc (0-8) et renvoie les indices du coin supérieur gauche du bloc.

```c#

public static (int row, int column) Corner(int block)
        {
            int r = -1, c = -1;

            if (block == 0 || block == 1 || block == 2)
                r = 0;
            else if (block == 3 || block == 4 || block == 5)
                r = 3;
            else if (block == 6 || block == 7 || block == 8)
                r = 6;

            if (block == 0 || block == 3 || block == 6)
                c = 0;
            else if (block == 1 || block == 4 || block == 7)
                c = 3;
            else if (block == 2 || block == 5 || block == 8)
                c = 6;

            return (r, c);
        }

```

Ainsi, lorsque chacune des neuf sous grilles est remplie, il faudra déterminer les erreurs.L'erreur totale pour une solution possible est la somme du nombre de valeurs manquantes dans les lignes, plus le nombre de valeurs manquantes dans les colonnes.     En raison de l'invariant d'algorithme, toutes les sous-grilles 3x3 n'ont aucune valeur manquante, elles ne contribuent donc pas à l'erreur. Ainsi, il faudra déterminer les erreurs en comptant le nombre de valeurs en double.


#### Générer une matrice de voisinage

Dans l’évolution combinatoire on a deux types d’objets organisme ceux qui utilisent la méthode matrice aléatoire et ceux qui utilisent une matrice voisine.

```c#

public static int[,] RandomMatrix(Random rnd, int[,] problem)
        {
            var result = DuplicateMatrix(problem);

            for (var block = 0; block < SIZE; ++block)
            {
                var corner = Corner(block);
                var values = Enumerable.Range(1, SIZE).ToList();

                for (var k = 0; k < values.Count; ++k)
                {
                    var ri = rnd.Next(k, values.Count);
                    var tmp = values[k];
                    values[k] = values[ri];
                    values[ri] = tmp;
                }

                var r = corner.row;
                var c = corner.column;
                for (var i = r; i < r + BLOCK_SIZE; ++i)
                {
                    for (var j = c; j < c + BLOCK_SIZE; ++j)
                    {
                        var value = problem[i, j];
                        if (value != 0)
                            values.Remove(value);
                    }
                }

                var pointer = 0;
                for (var i = r; i < r + BLOCK_SIZE; ++i)
                {
                    for (var j = c; j < c + BLOCK_SIZE; ++j)
                    {
                        if (result[i, j] != 0) continue;
                        var value = values[pointer];
                        result[i, j] = value;
                        ++pointer;
                    }
                }
            }

            return result;
        }

```

Pour déterminer cette matrice voisine, l’algorithme sélectionne un bloc au hasard puis deux cellules dans le bloc afin d’échanger les valeurs dans les deux cellules. On dira que deux grilles sont voisines l'une de l'autre si l'une peut s'obtenir à partir de l'autre en changeant un et un seul chiffre.

```c#

public static int[,] NeighborMatrix(Random rnd, int[,] problem, int[,] matrix)
        {
            // pick a random 3x3 block,
            // pick two random cells in block
            // swap values
            var result = DuplicateMatrix(matrix);

            var block = rnd.Next(0, SIZE); // [0,8]
            var corner = Corner(block);
            var cells = new List<int[]>();
            for (var i = corner.row; i < corner.row + BLOCK_SIZE; ++i)
            {
                for (var j = corner.column; j < corner.column + BLOCK_SIZE; ++j)
                {
                    if (problem[i, j] == 0)
                        cells.Add(new[] { i, j });
                }
            }

            if (cells.Count < 2)
                throw new Exception($"Block {block} doesn't have two values to swap!");

            // pick two. suppose there are 4 possible cells 0,1,2,3
            var k1 = rnd.Next(0, cells.Count); // 0,1,2,3
            var inc = rnd.Next(1, cells.Count); // 1,2,3
            var k2 = (k1 + inc) % cells.Count;

            var r1 = cells[k1][0];
            var c1 = cells[k1][1];
            var r2 = cells[k2][0];
            var c2 = cells[k2][1];

            var tmp = result[r1, c1];
            result[r1, c1] = result[r2, c2];
            result[r2, c2] = tmp;

            return result;
        }

```

#### Fusion de deux matrices

L’algorithme d’optimisation de l’évolution combinatoire implémente une évolution en sélectionnant deux objets Organism (les champs contiennent une erreur). On va utiliser ces 2 objets pour créer un nouvel objet unique, qui ne contient aucune erreur. 
On utilise la méthode MergeMatrices, qui accepte 2 matrices de dimensions 9x9. La méthode parcourt les blocs de 0 à 8, comme indique le code “ block < 9”. 
Pour chaque bloc, une valeur aléatoire est générée, comprise entre 0 et 1. Si la valeur aléatoire est inférieure à 0,5, les valeurs des deux blocs sont échangées. La méthode évolutive est comparée au mécanisme de croisement chromosomique (Déf : les chromosomes homologues appariés échangent des portions entre eux, on obtient donc des chromosomes recombinés). 
Le code est le suivant : 

```c#

public static int[,] MergeMatrices(Random rnd, int[,] m1, int[,] m2)
        {
            var result = DuplicateMatrix(m1);

            for (var block = 0; block < 9; ++block)
            {
                var pr = rnd.NextDouble();
                if (!(pr < 0.50)) continue;
                var corner = Corner(block);
                for (var i = corner.row; i < corner.row + BLOCK_SIZE; ++i)
                    for (var j = corner.column; j < corner.column + BLOCK_SIZE; ++j)
                        result[i, j] = m2[i, j];
            }

            return result;
         }

```

#### La méthode SolveEvo

La méthode SolveEvo (exécution de manière évolutive) est une combinaison de code et de pseudo-code (Déf : Le pseudo-code, appelé aussi Langage de Description d’Algorithmes, est une façon de décrire un algorithme en langage presque naturel, sans référence à un langage de programmation en particulier). La méthode commence par déterminer le nombre d'objets Organism comme 90% du nombre total utilisé. Cette valeur a été déterminée par les différents essais et erreurs et en trouvant les meilleures. Les objets Organism sont stockés dans un tableau nommé Hive. Nous voyons que la variable hive contient un nouvel organisme, qui va aussi chercher la meilleure erreur à son tour.
Le code est le suivant : 

```c#

private Sudoku SolveInternal(Sudoku sudoku, int numOrganisms, int maxEpochs)
        {
            var numberOfWorkers = (int)(numOrganisms * 0.90);
            var hive = new Organism[numOrganisms];

            var bestError = int.MaxValue;
            Sudoku bestSolution = null;

            for (var i = 0; i < numOrganisms; ++i)
            {
                var organismType = i < numberOfWorkers
                  ? OrganismType.Worker
                  : OrganismType.Explorer;

                var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));
                var err = randomSudoku.Error;

                hive[i] = new Organism(organismType, randomSudoku.CellValues, err, 0);

                if (err >= bestError) continue;
                bestError = err;
                bestSolution = Sudoku.New(randomSudoku);
            }

            var epoch = 0;
            while (epoch < maxEpochs)
            {
                if (epoch % 1000 == 0)
                    Console.WriteLine($"Epoch: {epoch}, Best error: {bestError}");

                if (bestError == 0)
                    break;

                for (var i = 0; i < numOrganisms; ++i)
                {
                    if (hive[i].Type == OrganismType.Worker)
                    {
                        var neighbor = MatrixHelper.NeighborMatrix(_rnd, sudoku.CellValues, hive[i].Matrix);
                        var neighborSudoku = Sudoku.New(neighbor);
                        var neighborError = neighborSudoku.Error;

                        var p = _rnd.NextDouble();
                        if (neighborError < hive[i].Error || p < 0.001)
                        {
                            hive[i].Matrix = MatrixHelper.DuplicateMatrix(neighbor);
                            hive[i].Error = neighborError;
                            if (neighborError < hive[i].Error) hive[i].Age = 0;

                            if (neighborError >= bestError) continue;
                            bestError = neighborError;
                            bestSolution = neighborSudoku;
                        }
                        else
                        {
                            hive[i].Age++;
                            if (hive[i].Age <= 1000) continue;
                            var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));
                            hive[i] = new Organism(0, randomSudoku.CellValues, randomSudoku.Error, 0);
                        }
                    }
                    else
                    {
                        var randomSudoku = Sudoku.New(MatrixHelper.RandomMatrix(_rnd, sudoku.CellValues));
                        hive[i].Matrix = MatrixHelper.DuplicateMatrix(randomSudoku.CellValues);
                        hive[i].Error = randomSudoku.Error;

                        if (hive[i].Error >= bestError) continue;
                        bestError = hive[i].Error;
                        bestSolution = randomSudoku;
                    }
                }
            }
        }

```

Il y a une stratégie d’optimisation mise en place pour que l’algorithme ne garde que des solutions optimales. A chaque boucle d’itération, chaque organisme génère une solution aléatoire. Ce qui crée un Organism unique. Parfois, la méthode ne va pas garder la meilleure solution mais prendre une solution moins optimale pour pousser l’algorithme a chercher une solution encore plus optimale. 


### Modifications apportées au programme

#### Implémentation du fichier sudoku.csv

Le fichier utilisé pour ce projet est à télécharger sur le lien suivant : https://www.kaggle.com/bryanpark/sudoku

Pour implémenter le fichier nous avons créé la static string "sudokusolution" qui transforme les données input de type string du fichier csv en matrice int[,] afin que la solution puisse lire correctement les sudokus. Puis une fois le sudoku résolu, elle le retransforme en string pour affichage et en output.


```c#

static string sudokusolution(string grid)
        {
            

            int[,] sudoku = new int[9, 9];
            
            for (int i = 0; i < 9; i++)
            {
                    
                for (int index = 0; index < 9; index++)
                {
                    sudoku[i, index] = Convert.ToInt32(grid[index]);
                }
            }


            Console.WriteLine("Begin solving Sudoku using combinatorial evolution");
            Console.WriteLine("The Sudoku is:");
            Console.WriteLine(sudoku.ToString());

            const int numOrganisms = 200;
            const int maxEpochs = 5000;
            const int maxRestarts = 40;
            Console.WriteLine($"Setting numOrganisms: {numOrganisms}");
            Console.WriteLine($"Setting maxEpochs: {maxEpochs}");
            Console.WriteLine($"Setting maxRestarts: {maxRestarts}");

            var solver = new SudokuSolver();
            var solvedSudoku = solver.Solve(sudoku, numOrganisms, maxEpochs, maxRestarts);

            Console.WriteLine("Best solution found:");
            Console.WriteLine(solvedSudoku.ToString());
            Console.WriteLine(solvedSudoku.Error == 0 ? "Success" : "Did not find optimal solution");
            Console.WriteLine("End Sudoku using combinatorial evolution");

            return Convert.ToString(solvedSudoku);
        }

```

#### Distribution dans Spark et parrallélisation dans un cluster

Afin de pouvoir distribué dans Spark, nous avons créé la static void "runSpark".
Dans un premier temps, on crée une session Spark où grâce au ".Config("spark.executor.cores", cores)" on peut choisir le nombre de noyaux du cluster, et ".Config("spark.executor.instances", nodes)" qui nous permet de choisir le nombre de noeuds.
Ensuite, on lit le fichier csv dans un DataFrame, que l'on va ensuite implémenter dans une fonction Spark Udf (User Defined Function) qui appelle la fonction vu juste avant "sudokusolution".

```c#

static void runSpark(string file_path, string cores, string nodes, int nrows)
        {

            // Create Spark session 
            SparkSession spark =
                SparkSession
                    .Builder()
                    .AppName("Resolution de " + nrows + " sudokus par évolution combinatoire de " + cores + " noyau(x) et " + nodes + " noeud(s)")
                    .Config("spark.executor.cores", cores)
                    .Config("spark.executor.instances", nodes)
                    .GetOrCreate();

            // Create initial DataFrame
            DataFrame dataFrame = spark
                .Read()
                .Option("header", true)
                .Option("inferSchema", true)
                .Schema("quizzes string, solutions string")
                .Csv(file_path);

            DataFrame dataFrame2 = dataFrame.Limit(nrows);

            spark.Udf().Register<string, string>(
                "SukoduUDF",
                (sudoku) => sudokusolution(sudoku));

            dataFrame2.CreateOrReplaceTempView("Resolved");
            DataFrame sqlDf = spark.Sql("SELECT quizzes, SukoduUDF(quizzes) as Resolution from Resolved");
            sqlDf.Show();

            spark.Stop();

        }

```

Enfin dans le static void Main, nous cherchons d'abord le fichier csv. Puis nous utilisons la fonction "System.Diagnostics.Stopwatch()" afin de pouvoir avoir une idée du temps d'exécution de notre solution. Nous définissons le nombre de noyaux et de noeuds que nous souhaitons dans notre cluster puis nous appelons "runSpark" en lui donnant les paramètres dont elle a bseoin à savoir le fichier à lire, le nombre de noyaux, le nombre de noeuds et le nombre de sudokus que nous souhaitons résoudre.

```c#

static void Main(string[] args)
        {
            
            string filepath = "/Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver/sudoku.csv";

            var temps1 = new System.Diagnostics.Stopwatch();

            var noyau = "1";
            var noeud = "2";
            var nbsudokus = 10;

            temps1.Start();
            runSpark(filepath, noyau, noeud, nbsudokus);
            temps1.Stop();

            Console.WriteLine($"Temps d'exécution pour " + noyau + " noyau et " + noeud + " noeud: " + temps1.ElapsedMilliseconds + " ms");

        }

```

### Commande d'exécution de la solution avec Spark-submit

Vous trouverez ci-dessous ainsi que en fichier .txt, les commandes à lancer dans le terminal pour les utilisateurs MacOS et dans l'invite de commande pour les utilisateurs Windows.
Il faut cependant modifier les chemins des différents objets au préalable.

MacOS :

```

export SPARK_HOME=/Users/sarahvasquez/Downloads/spark-2.4.1-bin-hadoop2.7 

export PATH="$SPARK_HOME/bin:$PATH"

export DOTNET_WORKER_DIR=/Users/sarahvasquez/Downloads/Microsoft.Spark.Worker-1.0.0 

cd /Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver

dotnet add package Microsoft.Spark

dotnet build

export DOTNET_ASSEMBLY_SEARCH_PATHS=/Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver/bin/Debug/netcoreapp3.1

spark-submit \
--class org.apache.spark.deploy.dotnet.DotnetRunner \
--master local \
/Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver/bin/Debug/netcoreapp3.1/microsoft-spark-2-4_2.11-1.0.0.jar  \
dotnet /Users/sarahvasquez/Desktop/Sudoku/SudokuCombinatorialEvolutionSolver/bin/Debug/netcoreapp3.1/SudokuCombinatorialEvolutionSolver.dll

```

Windows :

```

setx /M HADOOP_HOME C:\bin\spark-3.0.1-bin-hadoop2.7\
setx /M SPARK_HOME C:\bin\spark-3.0.1-bin-hadoop2.7\
setx /M PATH "%PATH%;%HADOOP_HOME%;%SPARK_HOME%\bin" 

setx /M DOTNET_WORKER_DIR <PATH-DOTNET-WORKER-DIR>

dotnet add package Microsoft.Spark

dotnet build

spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local C:\Users\vadaz\Downloads\wetransfer-bb1239\SudokuCombinatorialEvolutionSolver\bin\Debug\netcoreapp3.1\microsoft-spark-2-4_2.11-1.0.0.jar dotnet C:\Users\vadaz\Downloads\wetransfer-bb1239\SudokuCombinatorialEvolutionSolver\bin\Debug\netcoreapp3.1\SudokuCombinatorialEvolutionSolver.dll

```

