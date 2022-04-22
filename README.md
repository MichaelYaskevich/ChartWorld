# ChartWorld
Dive into charts with ChartWorld!

Список студентов-участников: Овчинников Павел КН202, Яскевич Михаил КН202, Шаламов Иван КН201, Дмитрий Лебедев КН203.

Проект решает проблему представления информации в виде различных диаграмм для получения удобного интерфеса манипулирования данными. 
Основные сценарии использования: 
1)  Инвестор хочет узнать соотношение своих активов, для этого в приложении создает новый Bar Chart и подает на вход cvs файл со строками вида "актив, стоимость". В результате, он может видеть соотношение своих активов ввиде соотношения столбцов разной величины.
2)  Теперь инвестор хочет увеличить долю акций Газпрома в своем портфеле, так чтобы соотношение активов его устраивало. Для этого он вытягивает столбец, отвечающий за Газпром, при этом стоимость акций Газпрома меняется соответствующим образом. В результате, он знает на какую стоимость он должен докупить акций Газпрома и продать других активов, чтобы при одной и такой же стоимости портфеля иметь лучшее соотношение активов.
3)  Теперь инвестор хочет поменять вид диаграммы со столбчатого на круглый, для этого он жмет на кнопку "поменять вид" и выбирает Pie Chart. В результате, вся информация будет отображена в виде Pie Chart.

Описание точек расширения:
1) Можно добавить ввод данных в другом виде. Для этого нужно написать адаптер который преобразует сырые данные к классу, который реализует интерфейс IData, тогда данные введенные в формате 2 смогут использоваться во всех местах где и данные введенные в формате 1.
2) Можно добавить новый вид диаграммы. Для этого нужно создать класс реализующий интерфейс IChart.
3) Можно добавить новый вид инструмента рабочей среды. Для этого нужно создать класс реализующий интерфейс IWorkspaceTool.

Проект состоит из четырёх частей:
1) Statistic (Mapping/Domain layer). Включает в себя создание различных способов ввода данных и подсчет различных видов статистики.
2) Chart (Domain layer). Включает в себя создание внутренних сущностей диаграмм, а также методов для их изменения.
3) Application (Application/Presentation layer). Включает в себя создание UI, включая отрисовку графа по заданным размеру и структуре.
4) Workspace (Domain layer). Включает в себя создание рабочего пространства, на котором можно разместить несколько графов и сохранить в базу данных или на компьютер их вид и расположение. Также реализовать подмножество функционала idroo, для того чтобы черкаться на графах , обводить их, писать текст и т.п. Речь идет о создании внутренних структур, а не отрисовки.
