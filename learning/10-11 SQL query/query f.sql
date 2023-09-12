select
    sum(count)
from
(SELECT
    clients.age as age,
    count(clients) as count
FROM clients
GROUP BY
    age) as ac
where count > 1