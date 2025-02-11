import { Link } from "react-router-dom";
import { useContext, useEffect, useState } from 'react';
import { Contex } from '../../../Context';
import axios from 'axios';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

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
    };

    const directionsByArea = trainingAreas.map(area => {
        return {
            areaName: area.name,
            directions: bachelorDirections.filter(direction =>
                area.code.startsWith(direction.code.slice(0, 2))
            )
        };
    });


    return (
        <Grid container spacing={3}>
            {directionsByArea.map((item, areaIndex) => (
                <Grid item xs={12} sm={6} md={4} key={areaIndex}>
                    <Typography variant="h5" gutterBottom>
                        {item.areaName}
                    </Typography>
                    <div>
                        {item.directions.map(direction => (
                            <Accordion key={direction.uuid}>
                                <AccordionSummary
                                    expandIcon={<ExpandMoreIcon />}
                                    aria-controls={`panel-${direction.uuid}-content`} // Уникальный id для aria-controls
                                    id={`panel-${direction.uuid}-header`} // Уникальный id для заголовка
                                >
                                    <Typography variant="body1" component="div"> {/* обертка div */}
                                        <Link to={`/${direction.uuid}`} onClick={() => takeAttributes(direction)} style={{ textDecoration: 'none', color: 'inherit' }}>
                                            {direction.name}
                                        </Link>
                                    </Typography>
                                </AccordionSummary>
                                <AccordionDetails>
                                    <Typography component={'span'} variant={'body2'}>
                                        {/* Здесь будет список специализаций */}
                                        Специализации:
                                        {direction.specializations && (
                                            <List>
                                                {direction.specializations.map((spec, index) => (
                                                    <ListItem key={index}>
                                                        <ListItemText primary={spec.name} />
                                                    </ListItem>
                                                ))}
                                            </List>
                                        )}
                                        {!direction.specializations && <div>Специализаций нет</div>}
                                    </Typography>
                                </AccordionDetails>
                            </Accordion>
                        ))}
                    </div>
                </Grid>
            ))}
        </Grid>
    );
}