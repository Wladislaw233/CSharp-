SELECT
    client.first_name Имя,
    client.last_name Фамилия,
    client.age Возраст
FROM public.client as client
GROUP BY
    client.age, client.first_name, client.last_name
ORDER BY
    client.age
