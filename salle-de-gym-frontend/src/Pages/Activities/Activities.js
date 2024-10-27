import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Container, Row, Col, Card } from 'react-bootstrap';
import { Link } from 'react-router-dom'; // Import Link from react-router-dom
import './Activities.css'; // Import your custom CSS file

const Activities = () => {
  const [activities, setActivities] = useState([]); // Store activities

  // Fetch activities from the API
  useEffect(() => {
    axios.get('https://localhost:7126/api/Activite')
      .then(response => {
        setActivities(response.data.$values); // Set the activities from the $values key
      })
      .catch(error => {
        console.error('Error fetching activities:', error);
      });
  }, []);

  return (
    <Container>
      <Row className="my-4">
        {Array.isArray(activities) && activities.map(activity => (
          <Col md={4} className="mb-4" key={activity.id}>
            <Link to={`/activities/${activity.id}`} style={{ textDecoration: 'none' }}>
              <Card className="activity-card">
                <Card.Img variant="top" src={activity.imageUrl} alt={activity.nom} />
                <Card.Body>
                  <Card.Title className="activity-card-title">{activity.nom}</Card.Title>
                  <Card.Text className="activity-card-text">
                    Capacity: {activity.capaciteMax} <br />
                    {activity.description}
                  </Card.Text>
                </Card.Body>
              </Card>
            </Link>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default Activities;
