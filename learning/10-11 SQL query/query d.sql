select
    clients.first_name,
    clients.last_name,
    currencies.code,
    accounts.amount
from accounts
left join clients on accounts.client_id = clients.client_id
left join currencies on accounts.currency_id = currencies.currency_id


