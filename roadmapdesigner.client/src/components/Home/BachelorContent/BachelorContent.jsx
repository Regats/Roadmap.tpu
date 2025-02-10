import { Link } from "react-router-dom";
import { useContext, useEffect, useState } from 'react';
import { Contex } from '../../../Context';
import axios from 'axios';
import Grid from '@mui/material/Grid'; // Сетка
import Typography from '@mui/material/Typography'; // Текст

export function BachelorContent() {
    const { setId, setTitle } = useContext(Contex);
    const [trainingAreas, setTrainingAreas] = useState([]);
    const [bachelorDirections, setBachelorDirections] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const trainingAreasResponse = await axios.get(`https://localhost:7244/api/direction-trainings`);
                const bachelorDirectionsResponse = await axios.get(`https://localhost:7244/api/direction-trainings`); // Замените на ваш API endpoint для бакалавриата

                if (trainingAreasResponse.status === 200 && bachelorDirectionsResponse.status === 200) {
                    setTrainingAreas(trainingAreasResponse.data);
                    setBachelorDirections(bachelorDirectionsResponse.data.flatMap(area => area.trainingDirections)); // Извлекаем направления бакалавриата
                } else {
                    console.error("Ошибка при загрузке данных с сервера");
                }
            } catch (error) {
                console.error("Ошибка при загрузке данных:", error);
            }
        }

        fetchData();
    }, []);

    const takeAttributes = (direction) => {
        setTitle(`${direction.code} ${direction.name}`);
        setId(direction.uuid);
    }

    return (
        <Grid container spacing={3}> {/* spacing задает отступы между элементами сетки */}
            {trainingAreas.map(area => (
                <Grid item xs={12} key={area.code}>
                    <Typography variant="h5" gutterBottom>
                        {area.name}
                    </Typography>
                    <Grid container spacing={2}>
                        {bachelorDirections
                            .filter(direction => area.code.startsWith(direction.code.slice(0, 2)))
                            .map(direction => (
                                <Grid item xs={12} sm={6} md={4} key={direction.uuid}>
                                    <Link
                                        to={`/${direction.uuid}`}
                                        onClick={() => takeAttributes(direction)}
                                        style={{ textDecoration: 'none', color: 'black' }} // Убираем подчеркивание
                                    >
                                        <Typography variant="body1">
                                            {direction.name}
                                        </Typography>
                                    </Link>
                                </Grid>
                            ))}
                    </Grid>
                </Grid>
            ))}
        </Grid>
    );
}