package com.greenfoxacademy.greenfoxacademy.repositories;

import com.greenfoxacademy.greenfoxacademy.models.ToDo;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Component;

@Component
public interface ToDoRepository extends JpaRepository<ToDo, Long> { }
