select
    clients.first_name,
    clients.last_name,
    accounts.amount
from clients
left join accounts
    on clients.client_id = accounts.client_id
where
    accounts.amount < 10000
order by
    amount