import { editTask, deleteTask } from '../api/TaskApi'
import { getTask } from '../api/TaskApi'
import { useAuth } from '../auth/AuthContext'
import { useState, useEffect } from 'react'
import TagList from '../tags/TagList';

export default function TaskDescription({ todoListId, taskId, setEditedTaskId }) {
    const { token } = useAuth();

    const [task, setTask] = useState({
        title: "",
        description: "",
        startDate: "",
        endDate: "",
        isCompleted: ""
    });

    useEffect(() => {
        getTask(todoListId, taskId, token)
            .then(data => {
                setTask(data);
            });
    }, [taskId]);

    function handleSubmitEdit(event) {
        event.preventDefault(event);
        
        editTask(todoListId, taskId, task, token);
        setEditedTaskId(taskId);
    }

    return (
        <div
            className="container-fluid border border-primary col">
            
            {taskId != -1 && <div>
                <h1>Task - {task.title}</h1>
                
                <form onSubmit={handleSubmitEdit}>
                    <label htmlFor="title">Title</label>
                    <input
                        id="title"
                        type="text"
                        name="title"
                        value={task.title}
                        onChange={(e) => { setTask({...task, title: e.target.value})}}></input>
                    
                    
                    <label htmlFor="description">Description</label>
                    <input
                        id="description"
                        type="text"
                        name="description"
                        value={task.description}
                        onChange={(e) => { setTask({ ...task, description: e.target.value }) }}></input>
                    
                    <button type="submit">Submit</button>
                </form>


                <div>
                    <TagList
                        setTag
                    />
                </div>
            </div>}
            
        </div>
    )
}