
SELECT client.first_name Имя,
       client.last_name Фамилия,
       account.currency_name Валюта,
       account.amount Остаток
FROM public.account as account
          LEFT JOIN public.client AS client
                   ON client.id = account.client_id
where
    account.amount = (select
                          MIN(amount)
                      from account)