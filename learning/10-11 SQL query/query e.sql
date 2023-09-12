select
    clients.first_name,
    clients.last_name,
    clients.date_of_birth,
    clients.age
from clients
order by
    age desc