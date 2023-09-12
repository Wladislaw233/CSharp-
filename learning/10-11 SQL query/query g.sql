/*не понял до конца задания, если группировать клиентов по возрасту,
  то и по другим полям придется, если нужна еще какая-то информация о клиенте*/
select
    clients.age,
    /*clients.first_name,*/
    count(clients) as count
from clients
group by
    age
    /*,first_name*/
order by
    count desc
