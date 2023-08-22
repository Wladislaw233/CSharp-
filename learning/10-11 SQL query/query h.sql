SELECT
    client.first_name Имя,
    client.last_name Фамилия,
    client.date_of_birth Дата_рождения
FROM public.client
LIMIT 3