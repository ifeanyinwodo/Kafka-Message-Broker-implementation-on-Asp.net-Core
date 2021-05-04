# Kafka-Message-Broker-implementation-on-Asp.net-Core
Kafka Message Broker implementation on Asp.net Core
Kafka Zookeeper Setup:
1.	Download and install latest version of Java 
2.	Setup JAVA_HOME as part of your system environment variables

Zookeeper Installation
You can Use this method to install Zookeeper or the one attached to Kafka Below
1.	Download Zookeeper => https://zookeeper.apache.org/releases.html#download
2.	Unzip (preferably use 7zip)
3.	goto the conf directory, make a copy of the sample zoo config file(.cfg), rename it to zoo.cfg and update it's content.
4.	go to bin directory and run the file zkServer.cmd

Kafka Installation
1.	Download Kafka => https://kafka.apache.org/downloads.html
2.	Unzip (preferably use 7zip)
3.	Go to the config directory, modify the file server.properties and add log directory.
Note: if you want to run zookeeper from kafka and not by using the above steps then modify the zookeeper.properties in the folder(config) appropriately.
4.	Navigate to the extracted Kafka Directory:
 

5.	Run the following to start zookeeper from kafka if you are not using the above option. \bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties
 
6.	Run the following to start kafka.
.\bin\windows\kafka-server-start.bat .\config\server.properties
 


Create Kafka Topic
Navigate to the  windows directory of your kafka directory and run the command below, specifying   the name you want to use for the topic, ours is microservice-topic
kafka-topics.bat --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic microservice-topic
 
 

Start Producer
Navigate to the windows directory of your kafka directory and run the command below, specifying   the topic you created earlier (microservice-topic)
kafka-console-producer.bat --broker-list localhost:9092 --topic microservice-topic
 

Start Consumer
Navigate to the windows directory of your kafka directory and run the command below, specifying   the topic you created earlier (microservice-topic)
kafka-console-consumer.bat --bootstrap-server localhost:9092 --topic microservice-topic --from-beginning
 









