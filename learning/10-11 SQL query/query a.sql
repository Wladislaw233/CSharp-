SELECT
    client.first_name Имя,
    client.last_name Фамилия,
    account.currency_name Валюта,
    account.amount Остаток
FROM public.client as client
LEFT JOIN public.account AS account
    ON account.client_id = client.id
WHERE
    account.amount < 10000
ORDER BY
    account.amount DESC;