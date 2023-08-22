SELECT
    client.first_name Имя,
    client.last_name Фамилия,
    client.date_of_birth Дата_рождения,
    client.age Возраст
FROM public.client AS client
ORDER BY
    client.date_of_birth