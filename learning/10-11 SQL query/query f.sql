SELECT
    client.age Возраст,
    count(client.first_name) Количество
FROM public.client as client
GROUP BY
    client.age