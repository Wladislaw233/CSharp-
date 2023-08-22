select client.first_name Имя,
       client.last_name Фамилия,
       account.currency_name Валюта,
       account.amount Остаток
from public.client as client
         RIGHT JOIN public.account as account on client.id = account.client_id

