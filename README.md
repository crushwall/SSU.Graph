# Graph

## Минимальные требования для класса Граф

Для решения всех задач курса необходимо создать класс (или иерархию классов - на усмотрение разработчика), содержащий:

Структуру для хранения списка смежности графа (не работать с графом через матрицы смежности, если в некоторых алгоритмах удобнее использовать список ребер - реализовать метод, преобразующий список смежности в список ребер);

### Конструкторы (не менее 3-х):
- :white_check_mark: конструктор по умолчанию, создающий пустой граф
- :white_check_mark: конструктор, заполняющий данные графа из файла
- :black_square_button: конструктор-копию
- :black_square_button: специфические конструкторы для удобства тестирования

### Методы:

- :white_check_mark: добавляющие вершину,
- :white_check_mark: добавляющие ребро (дугу),
- :white_check_mark: удаляющие вершину,
- :white_check_mark: удаляющие ребро (дугу),
- :white_check_mark: выводящие список смежности в файл (в том числе в пригодном для чтения конструктором формате).

Должны поддерживаться как ориентированные, так и неориентированные графы. Заранее предусмотрите возможность добавления меток и\или весов для дуг. Поддержка мультиграфа не требуется.

Добавьте минималистичный интерфейс пользователя (не смешивая его с реализацией!), позволяющий добавлять и удалять вершины и рёбра (дуги) и просматривать текущий список смежности графа.
### Замечание:
В зависимости от выбранного способа хранения графа могут появиться дополнительные трудности при удалении-добавлении, например, необходимость переименования вершин, если граф хранится списком (например, vector C++, List C#). Этого можно избежать, если хранить в списке пару (имя вершины, список смежных вершин), или хранить в другой структуре (например, Dictionary C#, map в С++). Идеально, если в качестве вершины реализуется обобщенный тип (generic).

Прислать ответ после проверки преподавателем в виде:
- файла с классом
- файла с демонстрацией использования конструкторов и методов добавления
- удаления вершин-ребер
- примеров входных файлов.

___

### Задание 1a

:white_check_mark: № 16. Выяснить, соседствуют ли две заданные вершины графа с одной общей вершиной. Вывести такую вершину.

:white_check_mark: № 17. Определить, можно ли попасть из вершины u в вершину v через одну какую-либо вершину орграфа. Вывести такую вершину.

### Задание 1b

:white_check_mark: № 10. Построить орграф, являющийся симметрической разностью двух заданных.

### Задание 2a

:black_square_button: № 22. Проверить, можно ли из орграфа удалить какую-либо вершину так, чтобы получилось дерево.

### Задание 2b

:black_square_button: № 28. Распечатать самый короткий (по числу рёбер) из путей от u до остальных вершин.

### Задание 4a

:black_square_button: № 11. Эксцентриситет вершины — максимальное расстояние из всех минимальных расстояний от других вершин до данной вершины. Радиус графа — минимальный из эксцентриситетов его вершин. Найти центр графа — множество вершин, эксцентриситеты которых равны радиусу графа.